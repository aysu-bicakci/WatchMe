import requests
import csv

API_KEY = '06aa759d714112a56cfa3b9c5fc7ab3a'
BASE_URL = 'https://api.themoviedb.org/3'

FIELDS = ['TVShowId', 'GenreId']


def get_tv_shows(page):
    url = f"{BASE_URL}/tv/popular"
    params = {
        'api_key': API_KEY,
        'language': 'en-US',
        'page': page
    }
    response = requests.get(url, params=params)
    if response.status_code == 200:
        return response.json()
    else:
        print(f"Error: {response.status_code}")
        return None


def save_to_csv(data, filename="TVShowGenres.csv"):
    with open(filename, mode='w', newline='', encoding='utf-8') as file:
        writer = csv.DictWriter(file, fieldnames=FIELDS)
        writer.writeheader()
        writer.writerows(data)


def main():
    all_genres = []
    page = 1
    target_tv_show_count = 1500  # Toplam çekilecek TV şovu sayısı

    while len(all_genres) < target_tv_show_count:
        print(f"Fetching page {page}...")
        tv_shows_data = get_tv_shows(page)
        if not tv_shows_data or 'results' not in tv_shows_data:
            break

        for tv_show in tv_shows_data.get('results', []):
            tv_show_id = tv_show.get('id')
            genres = tv_show.get('genre_ids', [])
            for genre_id in genres:
                all_genres.append({
                    'TVShowId': tv_show_id,
                    'GenreId': genre_id
                })

            if len(all_genres) >= target_tv_show_count:
                break

        page += 1

    print(f"Fetched genres for {len(all_genres)} TV shows.")
    save_to_csv(all_genres)
    print("TVShowGenres saved to TVShowGenres.csv")


if __name__ == "__main__":
    main()
