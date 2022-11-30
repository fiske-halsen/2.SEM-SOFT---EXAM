import requests
import pyodbc
from pprint import pprint

def get_zips():
    response = requests.get("https://api.dataforsyningen.dk/postnumre")

    cities_info = [{"zip":x['nr'], "city":x['navn']} for x in response.json()]

    with get_connection() as con:
        with con.cursor() as cur:
            #Tjek om tablename og columnnames er korrekte i forhold til databasen
            [cur.execute(f'insert into CityInfo (ZipCode, CityName) values ({x["zip"]}, {x["city"]}) ') for x in cities_info]


def get_connection():
    #Indsæt den korrete connection string
    return pyodbc.connect("CONNECTIONSTRING")

