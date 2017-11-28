import cPickle
import os
import sys
from os.path import join
import sqlite3

folderToSearch = './data_preprocessed_python/'
selected_folder = './selected_train_data/'
number_tags = {,
	"Todo": 0,
	"LALV": 0,
	"LAHV": 0,
	"HALV": 0,
	"HAHV": 0,
	"YoLALV": 0,
	"YoLAHV": 0,
	"YoHALV": 0,
	"YoHAHV": 0
}

def write_sql_header(Emotype):
	with sqlite3.connect(join(selected_folder, Emotype+'.db')) as dbConn:
		cur = dbConn.cursor()
		cur.execute('''CREATE TABLE data 
			(id INTEGER PRIMARY KEY AUTOINCREMENT, secs INT, it INT, F3 REAL, C4 REAL, SESSIONID INT)''')

def write_sql(Emotype, f3, c4, sessid):
	with sqlite3.connect(join(selected_folder, Emotype+'.db')) as dbConn:
		cur = dbConn.cursor()
		for idata in range(0, len(f3)):
			f3_value = f3[idata]
			c4_value = c4[idata]
			cur.execute('''INSERT INTO data(secs, it, F3, C4, SESSIONID) VALUES(?, ?, ?, ?, ?)''', (int(idata/128), int(idata%128), f3_value, c4_value, sessid))
		dbConn.commit()

def process_file(root, file, ifile):
	x = cPickle.load(open(join(root, file), 'rb'))
	for itrial in range(0, len(x['data'])):
		sessid = (ifile * 100) + itrial
		valence = x['labels'][itrial][0]
		arousal = x['labels'][itrial][1]
		dominance = x['labels'][itrial][2]
		liking = x['labels'][itrial][3]
		min_length = 4224
		max_length = 5396
		f3 = x['data'][itrial][2][min_length:max_length]
		c4 = x['data'][itrial][24][min_length:max_length]
		number_tags["Todo"]
		if(len(f3) != 1172 or len(c4) != 1172):
			print "error grave el largo de c3 o f4 no coinciden en"+file
			sys.exit()
		else:
			if arousal < 5 and valence < 5:
				number_tags["LALV"] += 1
				write_sql("LALV", f3, c4, sessid)
				#LALV
			if arousal < 5 and valence >= 5:
				number_tags["LAHV"] += 1
				write_sql("LAHV", f3, c4, sessid)
				#LAHV
			if arousal >= 5 and valence < 5:
				number_tags["HALV"] += 1
				write_sql("HALV", f3, c4, sessid)
				#HALV
			if arousal >= 5 and valence >= 5:
				number_tags["HAHV"] += 1
				write_sql("HAHV", f3, c4, sessid)
				#HAHV

			if (arousal <= 5 and valence < 5) or ((arousal > 5 and arousal < 6) and (valence > 5 and valence < 6)):
				number_tags["YoLALV"] += 1
				write_sql("YoLALV", f3, c4, sessid)
			if arousal < 5 and valence >= 5:
				number_tags["YoLAHV"] += 1
				write_sql("YoLAHV", f3, c4, sessid)
			if arousal >= 6 and valence <= 5:
				number_tags["YoHALV"] += 1
				write_sql("YoHALV", f3, c4, sessid)
			if arousal >= 5 and valence >= 6:
				number_tags["YoHAHV"] += 1
				write_sql("YoHAHV", f3, c4, sessid)

def process_all():
	ifile = 0
	for root, dirs, files in os.walk(folderToSearch):
		for file in files:
			if file.lower().endswith('.dat'):
				process_file(root, file, ifile)
				ifile = ifile + 1


if __name__ == '__main__':
	print number_tags
	write_sql_header("LALV")
	write_sql_header("LAHV")
	write_sql_header("HAHV")
	write_sql_header("HALV")
	write_sql_header("YoLALV")
	write_sql_header("YoLAHV")
	write_sql_header("YoHAHV")
	write_sql_header("YoHALV")
	process_all()
	print number_tags