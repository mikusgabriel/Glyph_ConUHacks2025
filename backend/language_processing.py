import spacy
import spacy_cleaner
from spacy_cleaner import processing, Cleaner


# Load the small English model
nlp = spacy.load("en_core_web_lg")

pipeline = spacy_cleaner.Cleaner(
    nlp,
    processing.remove_stopword_token,
    processing.replace_punctuation_token,
    processing.mutate_lemma_token,
)


def most_important_word_in_sentence(sentence: list, target_word: str) -> str:
    print(sentence, target_word)
    cleaned_sentence = pipeline.clean(sentence)[0].split()
    print(cleaned_sentence)
    target_word_weight = nlp(target_word)
    closest_word = ""
    closest_weight = 0
    for words in cleaned_sentence:
        print(words)
       
        cleaned_word_weight = nlp(words)

        similarity = cleaned_word_weight.similarity(target_word_weight)
        if similarity > closest_weight:
            closest_weight = similarity
            print()
            closest_word = words

    return (closest_weight, closest_word)


def getClosestOccupation():

    sentence = ["I am a programmer"]
    target_word = "occupation"

    important_word = most_important_word_in_sentence(sentence, target_word)
    print(f"Sentence: {sentence}")
    print(f"Target word: '{target_word}'")
    print(f"Most important related word: '{important_word}'")

def getRelatedSigns():
    pass



if __name__ == "__main__":
    getClosestOccupation()
