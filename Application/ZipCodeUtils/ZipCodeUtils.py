
import traceback
import requests
import pyodbc
from pprint import pprint

def get_zips():
    response = requests.get("https://api.dataforsyningen.dk/postnumre")

    cities_info = [{"zip":x['nr'], "city":x['navn']} for x in response.json()]

    pprint(cities_info)
        
 

get_zips()