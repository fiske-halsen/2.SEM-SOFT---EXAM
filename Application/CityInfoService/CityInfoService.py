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
        connection = ("Driver={SQL Server Native Client 11.0};"
                      "Server=127.0.0.1,5435;"
                      "Database=Orders;"
                      "User Id=sa;"
                      "Password=S3cur3P@ssW0rd!"
                      "Trusted_Connection=yes;")
    if args.production:
        connection = "Server=host.docker.internal,5435;Database=Orders;User Id=sa;Password=S3cur3P@ssW0rd!;TrustServerCertificate=True"

    response = requests.get("https://api.dataforsyningen.dk/postnumre")

    cities_info = [{"zip":x['nr'], "city":x['navn']} for x in response.json()]
    try:
        with get_connection(connection) as con:
            with con.cursor() as cur:
                print('inserted')
                #Tjek om tablename og columnnames er korrekte i forhold til databasen
                #[cur.execute(f'insert into CityInfo (ZipCode, CityName) values ({x["zip"]}, {x["city"]}) ') for x in cities_info]
    except UnboundLocalError:
        import sys
        print('Module can only be run with arguemnts --production or --development')
        sys.exit(1)



if __name__ == "__main__":
    insert_zips()