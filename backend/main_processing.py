from mistralai import Mistral
import os
from dotenv import load_dotenv
import spacy
import json

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


def getWordsFromOccupation(occupation):
    nlp = spacy.load("en_core_web_lg")

    with open("gestures_names.json", "r") as file:
        gestures_names = json.load(file)

    user_occupation_weight = nlp(occupation)

    highest_key = ""
    highest_weight = 0
    for key in list(gestures_names.keys()):
        print(key)
        parent_weight = nlp(key)
        if parent_weight.similarity(user_occupation_weight) > highest_weight:
            highest_key = key
            highest_weight = parent_weight

    return gestures_names[highest_key]


def getAllWords():
    return [
        # 15 words for a PARAMEDIC
        "AMBULANCE",
        "EMERGENCY",
        "HELP",
        "PAIN",
        "INJURY",
        "MEDICINE",
        "HOSPITAL",
        "BLOOD",
        "BREATHE",
        "ACCIDENT",
        "ALLERGIC",
        "HEART",
        "SHOCK",
        "STRETCHER",
        "BANDAGE",

        # 15 words for a TEACHER
        "TEACH",
        "LEARN",
        "SCHOOL",
        "CLASS",
        "LESSON",
        "HOMEWORK",
        "STUDENT",
        "BOOK",
        "TEST",
        "QUIET",
        "READ",
        "WRITE",
        "LISTEN",
        "EXPLAIN",
        "GRADE",

        # 15 words for a DOCTOR
        "DOCTOR",
        "CHECKUP",
        "PATIENT",
        "PRESCRIPTION",
        "SURGERY",
        "DIAGNOSE",
        "EXAM",
        "SPECIALIST",
        "SICK",
        "FEVER",
        "SYMPTOM",
        "CURE",
        "DISEASE",
        "RECOVERY",
        "THERAPY",

        # 55 OTHER USEFUL WORDS
        "HELLO",
        "THANK",
        "PLEASE",
        "SORRY",
        "NAME",
        "DEAF",
        "HEARING",
        "SIGN",
        "LANGUAGE",
        "FRIEND",
        "FAMILY",
        "MOTHER",
        "FATHER",
        "SIBLING",
        "LOVE",
        "FOOD",
        "WATER",
        "BATHROOM",
        "WHERE",
        "WHO",
        "WHAT",
        "WHEN",
        "WHY",
        "HOW",
        "YES",
        "NO",
        "WANT",
        "NEED",
        "GO",
        "COME",
        "LIKE",
        "DISLIKE",
        "HAPPY",
        "SAD",
        "TIRED",
        "EXCITED",
        "TIME",
        "DAY",
        "NIGHT",
        "MORNING",
        "AFTERNOON",
        "EVENING",
        "WORK",
        "HOME",
        "PHONE",
        "COMPUTER",
        "CAR",
        "BUS",
        "TRAIN",
        "RESTAURANT",
        "ORDER",
        "EAT",
        "DRINK",
        "EXCUSE",
        "FINISH"
    ]


