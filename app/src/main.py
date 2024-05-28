import logging
import requests
import time
import sys
import os
import ast
import store.files
import store.parquet
from ui import query as qu
import store


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

    data = imbalance_prices()

    out_name = qu.get_price_file_name(1)
    table = store.parquet.system_prices(data)
    store.files.write_table(table, out_name)


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
    year, month, day = qu.get_settlement_date()
    price_url = qu.get_price_url()

    run_url = (f'{price_url}/{year}/{month}/{day}')
    log.info(f'Url: {run_url}')

    resp = requests.get(run_url)
    data = resp.json().get('Value')

    # must convert the string (json is a string here) into a List of Dicts.
    return ast.literal_eval(data)

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