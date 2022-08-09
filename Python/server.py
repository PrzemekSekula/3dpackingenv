import argparse

import os
import sys

import socket
import threading
import json

from urllib.parse import unquote

from time import time

parser = argparse.ArgumentParser(description='Runs the BSM server')

parser.add_argument('--host',
                    type=str,
                    default='127.0.0.1',
                    help='IP of the host with the BSM server. Default=127.0.0.1'
                   )

parser.add_argument('-p', '--port',
                    type=int,
                    default=65432,
                    help='BSM server port. Default=65432'
                   )

parser.add_argument('--data_buffer',
                    type=int,
                    default=4096,
                    help='Size of data buffer in bytes. Default=4096'
                   )

parser.add_argument('--object_lifetime',
                    type=int,
                    default=20,
                    help='Object lifetime in seconds. Default=20'
                   )

parser.add_argument('--end_thread_time',
                    type=int,
                    default=10,
                    help="end a thread if didn't receive anything for X seconds. Default=100"
                   )

parser.add_argument('--ok',
                    type=str,
                    default='Ok.',
                    help="Ok message. Default=Ok."
                   )

parser.add_argument('-v', '--verbose',
                    type=int,
                    default=2,
                    help="Verbose for debugging purposes"
                   )

# Global response to be stored
response = None
action = None

def setAction(request):
    global action
    action = json.loads(request['action'])
    if args.verbose >= 2:
        print ("Action received:", type(action), action)
        
    return args.ok

def getAction(conn, request):
    global action
    if action is None:
        msg = str.encode('None')
    else:
        msg = str.encode(json.dumps(action))
        action = None

    conn.sendall(msg)


def setResponse(request):
    global response
    response = request['response']
    if args.verbose >= 2:
        print ("Response received:", type(response), response)
        
    return args.ok

def getResponse(conn, request):
    global response
    if response is None:
        msg = str.encode('None')
    else:
        msg = str.encode(response)
        response = None

    conn.sendall(msg)


def connection(nr, conn, addr):
    last_received = time()
    global buff
    global counter
    if args.verbose >= 1:
        print ("Connection nr: {}, c: {}, addr: {}".format(nr, conn, addr))
    while True:
        data = conn.recv(args.data_buffer)
        if args.verbose >= 3 :  
            print("Conn {}, addr: {}, Data received: {}".format(nr, addr, data))
        if len(data) == 0:
            if time() - last_received < args.end_thread_time:
                continue
            else:  # End thread if not conected for a while
                if args.verbose >= 1:
                    print ("\nDisconnecting session nr {}.".format(nr))
                break
        last_received = time()
        
        request = json.loads(data)
        if not "mode" in request:
            conn.sendall(str.encode('ERROR: No "mode" in request.'))
            continue
        if request['mode'] == 'setAction':
            ret = setAction(request)
        elif request['mode'] == 'getAction':
            ret = getAction(conn, request)            
        elif request['mode'] == 'setResponse':
            ret = setResponse(request)
        elif request['mode'] == 'getResponse':
            ret = getResponse(conn, request)            
        else:
            conn.sendall(str.encode('ERROR: Value for "mode" unknown.'))

if __name__ == '__main__':
    connr = 0
    args = parser.parse_args()
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        s.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        s.bind((args.host, args.port))
        s.listen()
        while True:
            conn, addr = s.accept()
            connr += 1
            t = threading.Thread(target=connection, args=(connr, conn, addr))
            t.start()

