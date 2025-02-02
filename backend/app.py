import json
from flask import Flask, request
from flask_sock import Sock  # <-- standard websockets for Flask
import json
import os
app = Flask(__name__)
sock = Sock(app)

tab = [
    # "HELP",
    # "ALLERGIC", "HEART", "SHOCK",  "BREATHE", "HELLO", "THANK", "PLEASE", "SORRY", "NAME",
    # "WHERE", "WHO", "WHAT", "WHEN", "WHY",
    # "HOW", "YES", "NO", "WANT", "NEED", "GO", "COME", "LIKE", "DISLIKE", "HAPPY", "SAD",
    "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K",
    "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "sorry", "thank",
    "help"
]


@sock.route('/meta')
def meta_websocket(ws):
    """
    This function handles WebSocket connections to /meta.
    """
    print("Meta Headset connected to /meta")

    list = []
    changed = False
    index = 0
    word_index = 26
    while True:
        # Receive a message (string) from the client
        raw_message = ws.receive()

        # If None, it usually means the client disconnected
        if raw_message is None:
            print("Meta Headset disconnected from /meta")
            break

        # print(f"Received message: {raw_message}")

        # Try to parse the message as JSON
        try:
            data = json.loads(raw_message)
        except json.JSONDecodeError:
            # If the client didn’t send JSON, handle as needed
            ws.send(json.dumps({"error": "Invalid JSON"}))
            continue

        # You can implement “event-based” logic:
        event_type = data["type"]
        del data["type"]

        if event_type == "hands_data":
            # print(f"Meta Headset sent hand data: {data}")
            # Possibly broadcast, handle logic, etc.
            print("WRITE", data["recording"])
            if data.get("recording", False) is True:

                print("WRITE", data["recording"])
                del data["recording"]

                list.append(data)

                changed = True

                letter = "WOrd"

                ws.send(json.dumps({
                    "status": "ok",
                    "type": "letter",
                    "letter": letter  # Include the actual data
                }))

            else:

                if changed:
                    if not os.path.exists(f"words/{tab[word_index]}/"):
                        os.makedirs(f"words/{tab[word_index]}/")
                    with open(f'words/{tab[word_index]}/sign_{str(index)}.json', 'w') as file:
                        json.dump(list, file, indent=4)
                    changed = False
                    list = []
                    index += 1
                    if index % 5 == 0 and index != 0:
                        word_index += 1
                        index = 0

                letter = "WOrd"
                for i in range(len(letter)):
                    letter

                ws.send(json.dumps({
                    "type": "letter",
                    "letter": letter,
                    "position": 1

                }))

        elif event_type == "start_occupation":
            print(f"Meta Headset sent hand data: {data}")
            # Possibly broadcast, handle logic, etc.
            ws.send(json.dumps({"status": "ok", "event": "start_occupation"}))

        elif event_type == "start_discover":
            print(f"Meta Headset sent hand data: {data}")
            # Possibly broadcast, handle logic, etc.
            ws.send(json.dumps({"status": "ok", "event": "start_discover"}))
        elif event_type == "start_conversation":
            print(f"Meta Headset sent hand data: {data}")
            # Possibly broadcast, handle logic, etc.
            ws.send(json.dumps(
                {"status": "ok", "event": "start_conversation"}))
        elif event_type == "hands_recording":
            print(f"Meta Headset sent hand data: {data}")
            # Possibly broadcast, handle logic, etc.

        elif event_type == "user_voice_command":
            print(f"Meta Headset sent voice command: {data}")
            # Implement your custom logic
            ws.send(json.dumps(
                {"status": "ok", "event": "voice_command_received"}))

        else:
            # A generic message fallback
            print(f"Unity /meta message event: {data}")
            ws.send(json.dumps(
                {"status": "ok", "event": "generic_message_received"}))


if __name__ == "__main__":
    app.run(port=3000)
