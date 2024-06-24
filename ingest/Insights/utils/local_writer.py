import os
import logging
import pathlib

def save_file(data, file_name):
    """
        write a file to the local machine.

        Args:
            data: the bytes to be streamed to a storage location
            file_system: the destination path
            file_name: to be created at the destination
        Return: 
            True on success else False
    """
    file_system = os.environ['WriteToContainer']

    home = pathlib.Path.home()
    save_dir = pathlib.Path.joinpath(home, file_system)
    save_file = pathlib.Path.joinpath(save_dir, file_name)

    try:
        with open(save_file, 'wb') as wr:
            wr.write(data)
        return True
    
    except Exception as ex:
        logging.error(f"Unable to write {save_file}.  Error: {ex}")
        return False
