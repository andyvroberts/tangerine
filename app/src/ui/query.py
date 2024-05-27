
import logging

log = logging.getLogger("app.src.ui.query.py")
#---------------------------------------------------------------------------------------#   
def get_settlement_date():
    """interact with the UI to find the query month

        Args:
            None:
            Return: a triple YYYY, mm, dd
    """
    return '2023', '12', '01'

#---------------------------------------------------------------------------------------#   
def get_price_url():
    """interact with the UI to find the imbalance price function URL

        Args:
            None:
            Return: the url of the locally running function app
    """
    return ' http://localhost:7071/api/bmrs/price'

#---------------------------------------------------------------------------------------#   
def get_volume_url():
    """interact with the UI to find the imbalance volume function URL

        Args:
            None:
            Return: the url of the locally running function app
    """
    return ' http://localhost:7071/api/bmrs/volume'


