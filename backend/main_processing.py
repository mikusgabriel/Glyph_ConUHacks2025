from mistralai import Mistral
import os
from dotenv import load_dotenv

load_dotenv()

mistral_key = os.getenv("MISTRAL_KEY")
print(mistral_key)

model = "mistral-small-latest"

client = Mistral(api_key=mistral_key)


def getOccupation(sentence):

    chat_response = client.chat.complete(
        model=model,

        messages=[{
            "role": "system",
            "content": "Extract the OCCUPATION/WORK/JOB of the person talking in the next sentence. Respond in one word"
        },
            {
                "role": "user",
                "content": f"{sentence}",
        },
        ]
    )

    print(chat_response.choices[0].message.content)

