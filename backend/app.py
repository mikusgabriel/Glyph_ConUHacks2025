import json
from flask import Flask, Response, request
from flask_socketio import SocketIO, Namespace
from flask_cors import CORS
import requests

app = Flask(__name__)
socketio = SocketIO(app)



@app.route("/",methods=["POST"])


# Namespace for Unity
class UnityNamespace(Namespace):
    def on_connect(self):
        print("Unity client connected to /unity")

    def on_disconnect(self):
        print("Unity client disconnected from /unity")

    def on_message(self, data):
        print(f"Unity /unity 'message' event: {data}")
        socketio.emit("message", data, namespace="/unity")

   


socketio.on_namespace(UnityNamespace("/unity"))


if __name__ == "__main__":
    # Run the Flask app with SocketIO
    socketio.run(app, host="0.0.0.0", port=8000, debug=True)
