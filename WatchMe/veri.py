import requests
import csv

API_KEY = '06aa759d714112a56cfa3b9c5fc7ab3a'
BASE_URL = 'https://api.themoviedb.org/3'

FIELDS = ['id', 'name', 'popularity', 'overview', 'first_air_date', 'poster_path']


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


def save_to_csv(data, filename="tv_shows.csv"):
    with open(filename, mode='w', newline='', encoding='utf-8') as file:
        writer = csv.DictWriter(file, fieldnames=FIELDS)
        writer.writeheader()
        writer.writerows(data)


def main():
    all_tv_shows = []
    seen_names = set()
    target_show_count = 800
    page = 1

    while len(all_tv_shows) < target_show_count:
        print(f"Fetching page {page}...")
        tv_data = get_tv_shows(page)
        if not tv_data or 'results' not in tv_data:
            break

        for show in tv_data.get('results', []):
            name = show.get('name')
            if name and name not in seen_names:
                seen_names.add(name)
                all_tv_shows.append({
                    'id': show.get('id'),
                    'name': name,
                    'popularity': show.get('popularity'),
                    'overview': show.get('overview'),
                    'first_air_date': show.get('first_air_date'),
                    'poster_path': f"https://image.tmdb.org/t/p/w500{show.get('poster_path')}" if show.get('poster_path') else ''
                })

                if len(all_tv_shows) >= target_show_count:
                    break

        page += 1

    print(f"Fetched {len(all_tv_shows)} unique TV shows.")
    save_to_csv(all_tv_shows)
    print("TV shows saved to tv_shows.csv")


if __name__ == "__main__":
    main()
