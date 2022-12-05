from ast import parse
import requests
import pyodbc
import argparse


def parse_args():
    parser = argparse.ArgumentParser()
    parser.add_argument('--development',action='store_true', help="When run with --development argument, the databse connection is connected to the local instance of the database")
    parser.add_argument('--production',action='store_true', help="When run with --production argument, the database connection is connected to the docker image of the host")
    return parser.parse_args()

def get_connection(connection):
    return pyodbc.connect(connection)

def insert_zips():
    args = parse_args()
    if args.development:
        connection = ("Driver=SQL Server;"
                      "Server=127.0.0.1,5435;"
                      "Database=Orders;"
                      "UID=sa;"
                      "PWD=S3cur3P@ssW0rd!")
    if args.production:
        connection = "Server=host.docker.internal,5435;Database=Orders;UID=sa;PWD=S3cur3P@ssW0rd!"

    response = requests.get("https://api.dataforsyningen.dk/postnumre")

    cities_info = [(x['nr'], x['navn']) for x in response.json()]
    
    with get_connection(connection) as con:
        with con.cursor() as cur:
            cur.executemany('insert into Cities (ZipCode, City) values(?,?)', cities_info)


if __name__ == "__main__":
    insert_zips()