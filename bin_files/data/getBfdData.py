import os
from os.path import join, getsize
import xml.etree.ElementTree as ET
import re
import pyedflib
import numpy as np

folderToSearch = './Sessions'
selected_folder = './selected_data/'

def process_file(Emotype,filename, onlyname):
	f = pyedflib.EdfReader(filename)
	#print 'n: ', f.signals_in_file
	#print 'labels: ', f.getSignalLabels()
	#print 'freq ', f.getSampleFrequencies() 
	f3i = 0
	c4i = 0
	i = 0
	for label in f.getSignalLabels():
		if label == u'F3':
			f3i = i
		if label == u'C4':
			c4i = i
		i = i+1
	with open(join(join(selected_folder, Emotype), Emotype+'.tsv'), 'a') as tsvFile:
		#tsvFile.write("Time\tIt\tF3\tC4\n")
		num_samples = f.getNSamples()[0]
		sigbufs = np.zeros((2, num_samples))
		sigbufs[0, :] = f.readSignal(f3i)
		sigbufs[1, :] = f.readSignal(c4i)
		j = 0
		total_seconds = num_samples / 256
		for i in np.arange(num_samples):
			secs_actual = i/256
			if (secs_actual <=30):
				continue
			if (secs_actual >= (total_seconds-30)):
				break
			tsvFile.write(str(int(secs_actual))+"\t"+str(i%256)+"\t"+str(sigbufs[0, i])+"\t"+str(sigbufs[1, i])+"\n")





def process_session(root_dir, filename, files):
	#print('dir: ', root_dir, ' file: ', filename)
	#print('files: ', files)
	tree = ET.parse(join(root_dir, filename))
	root = tree.getroot()
	felt_emo = root.get('feltEmo')
	if felt_emo == '3': #fear HALV
		for file in files:
			if file.lower().endswith('.bdf'):
				print('fear ', join(root_dir, file))
				process_file('HALV' ,join(root_dir, file), file)
	elif felt_emo == '6': #surprise HAHV
		for file in files:
			if file.lower().endswith('.bdf'):
				print('surprise ', join(root_dir, file))
				process_file('HAHV' ,join(root_dir, file), file)
	elif felt_emo == '11': #amusement LAHV
		for file in files:
			if file.lower().endswith('.bdf'):
				print('amusement ', join(root_dir, file))
				process_file('LAHV' ,join(root_dir, file), file)
	elif felt_emo == '2': #disgust LALV
		for file in files:
			if file.lower().endswith('.bdf'):
				print('disgust ', join(root_dir, file))
				process_file('LALV' ,join(root_dir, file), file)


def  main():
	i = 0
	for root, dirs, files in os.walk(folderToSearch):
		if 'session.xml' in files:
			process_session(root, 'session.xml', files)


if __name__ == '__main__':
    main()
