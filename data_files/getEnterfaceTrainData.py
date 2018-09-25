import os
from os.path import join, getsize
import re
import pyedflib
import numpy as np
import sqlite3
import traceback


folderToSearch = './EEG'
selected_folder = './selected_train_data/'
arousal_file = './Common/IAPS_Eval_Arousal_EEG_fNIRS.txt'
valence_file = './Common/IAPS_Eval_Valence_EEG_fNIRS.txt'

arousals = []
valences = []

def is_integer(s, base=10):
    try:
        val = int(s, base)
        return True
    except ValueError:
        return False

def is_float(s):
    try:
        val = float(s)
        return True
    except ValueError:
        return False


def get_values(filename):
    values = []
    with open(filename, 'r') as f:
        for line in f:
            vs = line.split('\t')
            for v in vs:
                if is_float(v):
                    values.append(float(v))
    return values


def decimation(signal, factor):
    if factor == 1:
        return signal
    newsignal = []
    counter_factor = 0
    x_tmp = 0
    for x in signal:
        counter_factor += 1
        if counter_factor == factor:
            x_tmp += x
            x_tmp = x_tmp / float(factor)
            newsignal.append(x_tmp)
            x_tmp = 0
            counter_factor = 0
        else:
            x_tmp += x
        #counter_factor += 1
    print("initial len: ", len(signal), ", proposed len: ", len(signal)/float(factor), ", new len: ", len(newsignal))
    return newsignal

#to accept all, put return -1
def inferior_limit_data(total_seconds):
    return (total_seconds/2)-4.5

#to accept all, put return total_seconds+1
def superior_limit_data(total_seconds):
    return (total_seconds/2)+4.5

def get_tag_yo(arousal, valence):
    if (arousal <= 5 and valence < 5) or ((arousal > 5 and arousal < 6) and (valence > 5 and valence < 6)):
        return 'YoLALV'
    if arousal < 5 and valence >= 5:
        return 'YoLAHV'
    if arousal >= 5 and valence <= 5:
        return 'YoHALV'
    if arousal >= 5 and valence >= 5:
        return 'YoHAHV'
    else:
        print("###########################################################################################")
        print("###########################################################################################")
        print("tag_yo")
        print(arousal, " - " , valence)
        print("###########################################################################################")
        print("###########################################################################################")

def get_tag_paper(arousal, valence):
    if arousal < 5 and valence < 5:
        return 'LALV'
    if arousal < 5 and valence >= 5:
        return 'LAHV'
    if arousal >= 5 and valence < 5:
        return 'HALV'
    if arousal >= 5 and valence >= 5:
        return 'HAHV'
    else:
        print("###########################################################################################")
        print("###########################################################################################")
        print("tag_paper")
        print(arousal, " - " , valence)
        print("###########################################################################################")
        print("###########################################################################################")

def get_tag_otro(arousal, valence):
    if (arousal >= 4 and arousal <= 6) or (valence >= 4 and valence <= 6):
        return 'ONeutral'
    if arousal < 4 and valence < 4:
        return 'OLALV'
    if arousal < 4 and valence > 6:
        return 'OLAHV'
    if arousal > 6 and valence < 4:
        return 'OHALV'
    if arousal > 6 and valence > 6:
        return 'OHAHV'
    else:
        print("###########################################################################################")
        print("###########################################################################################")
        print("tag_otro")
        print(arousal, " - " , valence)
        print("###########################################################################################")
        print("###########################################################################################")

def process_file(filename, file_number, factor):
    indices = []
    with open(filename+".mrk", 'r') as mrkf:
        i = 0
        start = 0
        end = 0
        for line in mrkf:
            if i > 1:
                vs = line.split('\t')
                if len(vs) == 0:
                    break
                if i % 2 ==0: #first
                    start = int(vs[1]) if is_integer(vs[1]) else -1
                else:
                    end = int(vs[1]) if is_integer(vs[1]) else -1
                    indices.append([start, end])
            i += 1
    #print("has: ", len(indices), " indices")
    #print(indices)
    with pyedflib.EdfReader(filename) as f:
        num_samples_total = f.getNSamples()[0]
        total_seconds = num_samples_total / (factor*256.0)
        print(file_number, ": ", filename,", tiene N: ",num_samples_total)
        print("minutes: ", total_seconds/60.0, ", secs: ", total_seconds)
        for i_indice in range(len(indices)):
            start = indices[i_indice][0]
            end = indices[i_indice][1]
            #print(start, " - ", end)
            #print(end-start)
            index_arrays = int((file_number * 30) + i_indice)
            #print(index_arrays)
            f3i = 0
            c4i = 0
            i = 0
            for label in f.getSignalLabels():
                if label == u'F3':
                    f3i = i
                if label == u'C4':
                    c4i = i
                i = i+1
            num_samples = end-start
            sigbufs2 = np.zeros((2, num_samples_total))
            sigbufs2[0, :] = f.readSignal(f3i)
            sigbufs2[1, :] = f.readSignal(c4i)
            sigbufs = np.zeros((2, int(num_samples/factor)))
            sigbufs[0, :] = decimation(sigbufs2[0, start:end], factor)
            sigbufs[1, :] = decimation(sigbufs2[1, start:end], factor)
            num_samples = num_samples / factor
            total_seconds = num_samples / 256.0
            if total_seconds <= 9.0:
                continue
            print("file: ", file_number,", en: ", i_indice, ", index: ", index_arrays, ", tiene N: ",num_samples)
            print("arousal: ", arousals[index_arrays], ", valence: ", valences[index_arrays])
            print("minutes: ", total_seconds/60.0, ", secs: ", total_seconds)
            arousal = -1
            valence = -1
            try:
                arousal = arousals[index_arrays]
                valence = valences[index_arrays]
            except:
                print("error at index: ", index_arrays)
            if arousal < 0 or valence < 0:
                print("////////////////////////////////////////////////////////////////////////////////////////////////////////")
                print("////////////////////////////////////////////////////////////////////////////////////////////////////////")
                print("negative arousal: ", arousal, " or valence: ", valence)
                print("////////////////////////////////////////////////////////////////////////////////////////////////////////")
                print("////////////////////////////////////////////////////////////////////////////////////////////////////////")
                continue
            try:
                Emotype = get_tag_yo(arousal, valence)
                print(Emotype)
                write_data_sqlite(sigbufs, num_samples, total_seconds, selected_folder, Emotype, index_arrays)
                Emotype = get_tag_paper(arousal, valence)
                print(Emotype)
                write_data_sqlite(sigbufs, num_samples, total_seconds, selected_folder, Emotype, index_arrays)
                Emotype = get_tag_otro(arousal, valence)
                print(Emotype)
                write_data_sqlite(sigbufs, num_samples, total_seconds, selected_folder, Emotype, index_arrays)
            except:
                print("-----------------------------------------------------------------------")
                print("EEEEEror al escribir a sql")
                print("file: ", file_number,", en: ", i_indice, ", index: ", index_arrays, ", tiene N: ",num_samples)
                traceback.print_exc()
                print()

def write_data_sqlite(sigbufs, num_samples, total_seconds, selected_folder, Emotype, session_id):
    with sqlite3.connect(join(selected_folder, Emotype+'.db')) as dbConn:
        cur = dbConn.cursor()
        for i in np.arange(num_samples):
            secs_actual = i/256.0
            if (secs_actual < inferior_limit_data(total_seconds)):
                continue
            if (secs_actual > superior_limit_data(total_seconds)):
                break
            cur.execute('''INSERT INTO data(secs, it, F3, C4, SESSIONID) VALUES(?, ?, ?, ?, ?)''', (int(secs_actual), int(i%256), float(sigbufs[0, int(i)]), float(sigbufs[1, int(i)]), int(session_id)))
        dbConn.commit()



def send_to_process(files, root_dir):
    i = 0
    with open("erroreslog.txt", "w") as error_f:
        for file in files:
            if file.lower().endswith('.bdf'):
                print(join(root_dir, file))
                try:
                    if i == 0:
                        process_file(join(root_dir, file), int(i/2), 1)
                    else:
                        process_file(join(root_dir, file), int(i/2), 4)
                except:
                    print("error en: ", file)
                    error_f.write(file+"\n")
                    error_f.flush()
            i += 1

def write_sql_header(Emotype):
    with sqlite3.connect(join(selected_folder, Emotype+'.db')) as dbConn:
        cur = dbConn.cursor()
        cur.execute('''CREATE TABLE data 
            (id INTEGER PRIMARY KEY AUTOINCREMENT, secs INT, it INT, F3 REAL, C4 REAL, SESSIONID INT)''')


def  main():
    i = 0
    for root, dirs, files in os.walk(folderToSearch):
        send_to_process(files, root)


if __name__ == '__main__':
    arousals = get_values(arousal_file)
    valences = get_values(valence_file)
    print(len(arousals))
    print(len(valences))
    print(arousals)
    print(valences)
    write_sql_header("LALV")
    write_sql_header("LAHV")
    write_sql_header("HAHV")
    write_sql_header("HALV")
    write_sql_header("YoLALV")
    write_sql_header("YoLAHV")
    write_sql_header("YoHAHV")
    write_sql_header("YoHALV")
    write_sql_header("OLALV")
    write_sql_header("OLAHV")
    write_sql_header("OHAHV")
    write_sql_header("OHALV")
    write_sql_header("ONeutral")
    main()