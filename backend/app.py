import json
from flask import Flask, Response, request
from flask_socketio import SocketIO, Namespace
from flask_cors import CORS
import requests

app = Flask(__name__)
socketio = SocketIO(app)


@app.route("/", methods=["POST"])
def send_message():
    try:
        json_message = request.json()

        message = json.loads(json_message)
        print(message)

        requests.post("localhost:5000")

    except json:
        print("error", json)


# Namespace for Unity
class MetaHeadsetNamespace(Namespace):
    def on_connect(self):
        print("Meta Headset connected to /meta")

    def on_disconnect(self):
        print("Meta Headset disconnected from /meta")

    def on_message(self, data):
        print(f"Unity /meta 'message' event: {data}")
        socketio.emit("message", data, namespace="/meta")

    def on_hand_data(self,data):
        print(f"Meta Headset sent hand data: {data}")


        socketio.emit("message",data,namespace="/meta")

    def on_user_voice_command(self,data):
        print(f"Meta Headset sent voice message from user:{data}")

        


        #decide to what to do with the transcripted message

socketio.on_namespace(MetaHeadsetNamespace("/meta"))


if __name__ == "__main__":
    # Run the Flask app with SocketIO
    socketio.run(app, host="0.0.0.0", port=8000, debug=True)
