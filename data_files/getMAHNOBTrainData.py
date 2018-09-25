import os
from os.path import join, getsize
import xml.etree.ElementTree as ET
import re
import pyedflib
import numpy as np
import sqlite3

folderToSearch = './Sessions'
selected_folder = './selected_train_data/'


def is_integer(s, base=10):
	try:
		val = int(s, base)
		return True
	except ValueError:
		return False

#to accept all, put return -1
def inferior_limit_data(total_seconds):
	return (total_seconds/2)-4.5

#to accept all, put return total_seconds+1
def superior_limit_data(total_seconds):
	return (total_seconds/2)+4.5

def process_file(Emotype, filename, session_id):
	with pyedflib.EdfReader(filename) as f:
		f3i = 0
		c4i = 0
		i = 0
		for label in f.getSignalLabels():
			if label == u'F3':
				f3i = i
			if label == u'C4':
				c4i = i
			i = i+1
		num_samples = f.getNSamples()[0]
		sigbufs = np.zeros((2, num_samples))
		sigbufs[0, :] = f.readSignal(f3i)
		sigbufs[1, :] = f.readSignal(c4i)
	total_seconds = num_samples / 256
	write_data_sqlite(sigbufs, num_samples, total_seconds, selected_folder, Emotype, session_id)

def write_data_sqlite(sigbufs, num_samples, total_seconds, selected_folder, Emotype, session_id):
	with sqlite3.connect(join(selected_folder, Emotype+'.db')) as dbConn:
		cur = dbConn.cursor()
		for i in np.arange(num_samples):
			secs_actual = i/256.0
			if (secs_actual < inferior_limit_data(total_seconds)):
				continue
			if (secs_actual > superior_limit_data(total_seconds)):
				break
			cur.execute('''INSERT INTO data(secs, it, F3, C4, SESSIONID) VALUES(?, ?, ?, ?, ?)''', (int(secs_actual), int(i%256), float(sigbufs[0, i]), float(sigbufs[1, i]), int(session_id)))
		dbConn.commit()

def write_data_cs(sigbufs, num_samples, total_seconds, selected_folder, Emotype, session_id):
	with open(join(selected_folder, Emotype+'.generated.cs'), 'a') as csFile:
		for i in np.arange(num_samples):
			secs_actual = i/256.0
			if (secs_actual < inferior_limit_data(total_seconds)):
				continue
			if (secs_actual > superior_limit_data(total_seconds)):
				break
			csFile.write("new double[]{"+str(int(secs_actual))+","+str(i%256)+","+str(sigbufs[0, i])+","+str(sigbufs[1, i])+"},\n")

def write_data_tsv(sigbufs, num_samples, total_seconds, selected_folder, Emotype, session_id):
	with open(join(selected_folder, Emotype+'.tsv'), 'a') as csFile:
		for i in np.arange(num_samples):
			secs_actual = i/256.0
			if (secs_actual < inferior_limit_data(total_seconds)):
				continue
			if (secs_actual > superior_limit_data(total_seconds)):
				break
			csFile.write(str(int(secs_actual))+"\t"+str(i%256)+"\t"+str(sigbufs[0, i])+"\t"+str(sigbufs[1, i])+"\n")

def send_to_process(files, root_dir, tag, session_id):
	for file in files:
		if file.lower().endswith('.bdf'):
			print(tag, join(root_dir, file))
			process_file(tag ,join(root_dir, file), session_id)


def process_session(root_dir, filename, files):
	tree = ET.parse(join(root_dir, filename))
	root = tree.getroot()
	session_id = str(root.get('sessionId'));
	felt_arousal = root.get('feltArsl')
	felt_valence = root.get('feltVlnc')
	felt_emo = str(root.get('feltEmo'))
	felt_control = str(root.get('feltCtrl'))
	felt_predictability = str(root.get('feltPred'))
	low_values = ['1', '2', '3', '4']
	high_values = ['5', '6', '7', '8', '9']
	all_values= ['1', '2', '3', '4', '5', '6', '7', '8', '9']
	threeal_low = ['1', '2', '3', '4']
	thereal_high = ['6', '7', '8', '9']
	if is_integer(session_id):
		if felt_emo in ['0', '1', '2', '3', '4', '5', '6', '11', '12']:
			send_to_process(files, root_dir, 'Emotion'+felt_emo, session_id)
		if felt_control in all_values:
			send_to_process(files, root_dir, 'Control'+felt_control, session_id)
		if felt_predictability in all_values:
			send_to_process(files, root_dir, 'Predictability'+felt_predictability, session_id)
		if felt_valence in all_values:
			send_to_process(files, root_dir, 'Valence'+felt_valence, session_id)
		if felt_arousal in all_values:
			send_to_process(files, root_dir, 'Arousal'+felt_arousal, session_id)
			
		if (felt_arousal in ['1', '2', '3', '4', '5'] and felt_valence in ['1', '2', '3', '4']) or (felt_arousal == '5' and felt_valence == '5'):
			send_to_process(files, root_dir, 'SegYo-LALV', session_id)
		if (felt_arousal in low_values) and (felt_valence in low_values):
			send_to_process(files, root_dir, 'SegmentationPaper-LALV', session_id)
		if (felt_arousal in threeal_low) and (felt_valence in threeal_low):
			send_to_process(files, root_dir, 'SegOLALV', session_id)
			#LALV
		if felt_arousal in ['1', '2', '3', '4'] and felt_valence in ['5', '6', '7', '8', '9']:
			send_to_process(files, root_dir, 'SegYo-LAHV', session_id)
		if (felt_arousal in low_values) and (felt_valence in high_values):
			send_to_process(files, root_dir, 'SegmentationPaper-LAHV', session_id)
		if (felt_arousal in threeal_low) and (felt_valence in thereal_high):
			send_to_process(files, root_dir, 'SegOLAHV', session_id)
			#LAHV
		if felt_arousal in ['6', '7', '8', '9'] and felt_valence in ['1', '2', '3', '4', '5']:
			send_to_process(files, root_dir, 'SegYo-HALV', session_id)
		if (felt_arousal in high_values) and (felt_valence in low_values):
			send_to_process(files, root_dir, 'SegmentationPaper-HALV', session_id)
		if (felt_arousal in thereal_high) and (felt_valence in threeal_low):
			send_to_process(files, root_dir, 'SegOHALV', session_id)
			#HALV
		if felt_arousal in ['5', '6', '7', '8', '9'] and felt_valence in ['6', '7', '8', '9']:
			send_to_process(files, root_dir, 'SegYo-HAHV', session_id)
		if (felt_arousal in high_values) and (felt_valence in high_values):
			send_to_process(files, root_dir, 'SegmentationPaper-HAHV', session_id)
		if (felt_arousal in thereal_high) and (felt_valence in thereal_high):
			send_to_process(files, root_dir, 'SegOHAHV', session_id)
			#HAHV
		if (felt_arousal in ['5']) or (felt_valence in ['5']):
			send_to_process(files, root_dir, 'SegONeutral', session_id)
	with(open('resumen.tsv', 'a')) as resumenFile:
		if felt_arousal in all_values and felt_valence in all_values:
			resumenFile.write(filename+"\t"+felt_arousal+"\t"+felt_valence+"\n")

def write_cs_header(Emotype):
	with open(join(selected_folder, Emotype+'.generated.cs'), 'w') as tsvFile:
		tsvFile.write('''using System;
using System.Collections.Generic;
using System.Text;

namespace cl.uv.leikelen.Module.Processing.EEGEmotion2Channels.Data
{
	public static class '''+Emotype +''' 
	{
		public static double[][] data = new double[][]{
			''')

def write_tsv_header(Emotype):
	with open(join(selected_folder, Emotype+'.tsv'), 'w') as tsvFile:
		tsvFile.write("secs\titeration\tF3\tC4\n")

def write_sql_header(Emotype):
	with sqlite3.connect(join(selected_folder, Emotype+'.db')) as dbConn:
		cur = dbConn.cursor()
		cur.execute('''CREATE TABLE data 
			(id INTEGER PRIMARY KEY AUTOINCREMENT, secs INT, it INT, F3 REAL, C4 REAL, SESSIONID INT)''')

def  main():
	i = 0
	for root, dirs, files in os.walk(folderToSearch):
		if 'session.xml' in files:
			process_session(root, 'session.xml', files)


if __name__ == '__main__':
	write_sql_header('SegmentationPaper-LALV')
	write_sql_header('SegmentationPaper-LAHV')
	write_sql_header('SegmentationPaper-HALV')
	write_sql_header('SegmentationPaper-HAHV')

	write_sql_header('SegYo-LALV')
	write_sql_header('SegYo-LAHV')
	write_sql_header('SegYo-HALV')
	write_sql_header('SegYo-HAHV')

	write_sql_header('SegOLALV')
	write_sql_header('SegOLAHV')
	write_sql_header('SegOHALV')
	write_sql_header('SegOHAHV')
	write_sql_header('SegONeutral')

	write_sql_header('Valence1')
	write_sql_header('Valence2')
	write_sql_header('Valence3')
	write_sql_header('Valence4')
	write_sql_header('Valence5')
	write_sql_header('Valence6')
	write_sql_header('Valence7')
	write_sql_header('Valence8')
	write_sql_header('Valence9')

	write_sql_header('Arousal1')
	write_sql_header('Arousal2')
	write_sql_header('Arousal3')
	write_sql_header('Arousal4')
	write_sql_header('Arousal5')
	write_sql_header('Arousal6')
	write_sql_header('Arousal7')
	write_sql_header('Arousal8')
	write_sql_header('Arousal9')

	write_sql_header('Control1')
	write_sql_header('Control2')
	write_sql_header('Control3')
	write_sql_header('Control4')
	write_sql_header('Control5')
	write_sql_header('Control6')
	write_sql_header('Control7')
	write_sql_header('Control8')
	write_sql_header('Control9')

	write_sql_header('Predictability1')
	write_sql_header('Predictability2')
	write_sql_header('Predictability3')
	write_sql_header('Predictability4')
	write_sql_header('Predictability5')
	write_sql_header('Predictability6')
	write_sql_header('Predictability7')
	write_sql_header('Predictability8')
	write_sql_header('Predictability9')

	write_sql_header('Emotion0')
	write_sql_header('Emotion1')
	write_sql_header('Emotion2')
	write_sql_header('Emotion3')
	write_sql_header('Emotion4')
	write_sql_header('Emotion5')
	write_sql_header('Emotion6')
	write_sql_header('Emotion11')
	write_sql_header('Emotion12')
	main()
