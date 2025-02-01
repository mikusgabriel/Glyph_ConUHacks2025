import json
from flask import Flask, request
from flask_sock import Sock  # <-- standard websockets for Flask

app = Flask(__name__)
sock = Sock(app)

@sock.route('/meta')
def meta_websocket(ws):
    """
    This function handles WebSocket connections to /meta.
    """
    print("Meta Headset connected to /meta")
    
    while True:
        # Receive a message (string) from the client
        raw_message = ws.receive()
        
        # If None, it usually means the client disconnected
        if raw_message is None:
            print("Meta Headset disconnected from /meta")
            break
        
        print(f"Received message: {raw_message}")

        # Try to parse the message as JSON
        try:
            data = json.loads(raw_message)
        except json.JSONDecodeError:
            # If the client didn’t send JSON, handle as needed
            ws.send(json.dumps({"error": "Invalid JSON"}))
            continue
        
        # You can implement “event-based” logic:
        event_type = data.get("event")
        payload = data.get("payload",{})

        if event_type == "hand_data":
            print(f"Meta Headset sent hand data: {payload}")
            # Possibly broadcast, handle logic, etc.
            ws.send(json.dumps({"status": "ok", "event": "hand_data_received"}))

        elif event_type == "user_voice_command":
            print(f"Meta Headset sent voice command: {payload}")
            # Implement your custom logic
            ws.send(json.dumps({"status": "ok", "event": "voice_command_received"}))

        else:
            # A generic message fallback
            print(f"Unity /meta message event: {data}")
            ws.send(json.dumps({"status": "ok", "event": "generic_message_received"}))

if __name__ == "__main__":
    # Run the Flask app (standard HTTP) + integrated WebSocket server
    app.run(host="0.0.0.0", port=5000, debug=True)
    
