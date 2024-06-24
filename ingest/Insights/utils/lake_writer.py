import os
import logging
from azure.storage.filedatalake import DataLakeFileClient as fc

# MS doc page for filedatalake
# https://docs.microsoft.com/en-us/python/api/overview/azure/storage-file-datalake-readme?view=azure-python
# pip install azure-storage-file-datalake --pre


def save_file(data, file_name):
    """
        write a file to the data lake.

        Args:
            data: the bytes to be streamed to a storage location
            conn_string: the ADLS connection string (DFS version)
            file_system: the container and namespace concatenation
            file_name: to be created at the destination
        Return: 
            True on success else False.
    """
    # set the azure logging to WARN to avoid excessive outputs
    az_st_log = logging.getLogger('azure.storage')
    az_st_log.setLevel(logging.WARN)
    az_cr_log = logging.getLogger('azure.core')
    az_cr_log.setLevel(logging.WARN)

    conn_string = os.environ['AzureWebJobsStorage']
    file_system = os.environ['WriteToContainer']

    try:
        file = fc.from_connection_string(conn_string, file_system_name=file_system, file_path=file_name)
        file.create_file()
        file.upload_data(data, length=None, overwrite=True)
        return True

    except Exception as ex:
        logging.error(f"Unable to write {file_system}/{file_name}.  Error: {ex}")
        return False
    