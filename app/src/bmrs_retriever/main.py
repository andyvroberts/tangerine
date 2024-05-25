import logging
import pandas as pd
import requests
import json
import time
import sys
import os

from react import query


log = logging.getLogger("bmrs_retriever.main.py")
#------------------------------------------------------------------------------
def controller():
    """
        Orchestrate the process

        Args:
            return: None
    """
    log.info("-------------------------------------------------------------------")
    start_exec = time.time()
    log.debug(f'executing from path {os.getcwd()}')

    price_df = imbalance_prices()

    end_exec = time.time()
    duration = end_exec - start_exec
    hours, hrem = divmod(duration, 3600)
    mins, secs = divmod(hrem, 60)
    log.info(f"Finished process: {hours}:{mins}:{round(secs, 2)}.")

#---------------------------------------------------------------------------------------#  
def imbalance_prices():
    """interact with the UI to find the imbalance price function URL

        Args:
            None:
            Return: the url of the locally running function app
    """
    year, month, day = query.get_settlement_date()
    price_url = query.get_price_url()

    run_url = (f'{price_url}/{year}/{month}/{day}')
    log.info(f'Url: {run_url}')

    resp = requests.get(run_url)

    parsed = json.loads(resp.content)

    log.info(f'response = {parsed}')
    #df = pd.read_json(resp)
    #print(df)

#------------------------------------------------------------------------------
if __name__ == '__main__' :
    logging.basicConfig(
        level=logging.DEBUG,
        format="%(asctime)s %(levelname)-8s [%(name)s]: %(message)s",
        datefmt='%Y-%m-%d %I:%M:%S',
        handlers = [
            logging.StreamHandler(sys.stdout)
        ]
    )

    controller()