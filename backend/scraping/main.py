import requests
from bs4 import BeautifulSoup
import csv


tab = [
    # "HELP",
    # "ALLERGIC", "HEART", "SHOCK",  "BREATHE", "HELLO", "THANK", "PLEASE", "SORRY", "NAME",
    # "WHERE", "WHO", "WHAT", "WHEN", "WHY",
    # "HOW", "YES", "NO", "WANT", "NEED", "GO", "COME", "LIKE", "DISLIKE", "HAPPY", "SAD",
    "A", "B", "C", "D", "E", "F", "G", "H", "I", "J"
    "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z","thank"
]

# URL of the website to scrape
url = "https://www.signasl.org/sign/"

data = []

for word in tab:
    if not word:
        print("done")
        break

    # Send a GET request to the website
    print(url, word.lower())
    response = requests.get(url+word.lower())
    if response.status_code != 200:
        print(
            f"Failed to retrieve the webpage. Status code: {response.status_code}")
        exit()

    # Parse the HTML content with BeautifulSoup
    soup = BeautifulSoup(response.text, "html.parser")

    videos = soup.find_all("video")

    video_urls = []
    i = 0
    for video in videos:
        if i == 2:
            break
        video_url = video.find("source").get("src")
        print(video_url)
        video_urls.append(video_url)
        i += 1

    temp_list = [word]
    for video_url_item in video_urls:
        temp_list.append(video_url_item)
    data.append(temp_list)
    response.close()

# Write the scraped data to a CSV file
csv_filename = "animations.csv"
with open(csv_filename, mode="w", newline="", encoding="utf-8") as csv_file:
    # Define the fieldnames (CSV header)
    fieldnames = ["name", "url1", "url2"]
    writer = csv.DictWriter(csv_file, fieldnames=fieldnames)

    # Write header
    writer.writeheader()

    # Write each row dynamically
    for item in data:
        row = {
            "name": item[0] if len(item) > 0 else "",  # First element or empty
            # Second element or empty
            "url1": item[1] if len(item) > 1 else "",
            "url2": item[2] if len(item) > 2 else "",  # Third element or empty
        }
        writer.writerow(row)

print(f"Scraped data has been written to {csv_filename}")
