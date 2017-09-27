
CREATE TABLE scene(
	scene_id SERIAL,
	number_of_participants INTEGER,
	name VARCHAR(50) NOT NULL,
	type VARCHAR(20),
	place VARCHAR(30),
	description VARCHAR(150),
	record_real_datetime TIMESTAMP WITH TIME ZONE,
	record_start_datetime TIMESTAMP WITHOUT TIME ZONE NOT NULL,
	duration TIME NOT NULL,
	PRIMARY KEY(scene_id)
);

CREATE TABLE person(
	person_id SERIAL,
	name VARCHAR(50) NOT NULL,
	photo VARCHAR(50),
	birthday DATE,
	sex CHAR,
	PRIMARY KEY(person_id)
);

CREATE TABLE person_in_scene(
	scene_id INTEGER NOT NULL,
	person_id INTEGER NOT NULL,
	PRIMARY KEY(scene_id, person_id),
	CONSTRAINT person_in_scene_scene_id_fkey FOREIGN KEY(scene_id) REFERENCES scene (scene_id),
	CONSTRAINT person_in_scene_person_id_fkey FOREIGN KEY(person_id) REFERENCES person (person_id)
);

CREATE TABLE modal_type(
	modaltype_name VARCHAR(50) NOT NULL,
	description VARCHAR(150),
	PRIMARY KEY(modaltype_name)
);

CREATE TABLE submodal_type(
	submodaltype_name VARCHAR(50) NOT NULL,
	modaltype_name VARCHAR(50) NOT NULL,
	description VARCHAR(150),
	file VARCHAR(50),
	PRIMARY KEY(submodaltype_name, modaltype_name),
	CONSTRAINT submodal_type_modal_type_name_fkey FOREIGN KEY(modaltype_name) REFERENCES modal_type (modaltype_name)
);

CREATE TABLE smt_pis(
	smt_pis_id SERIAL,
	scene_id INTEGER NOT NULL,
	person_id INTEGER NOT NULL,
	submodaltype_name VARCHAR(50) NOT NULL,
	modaltype_name VARCHAR(50) NOT NULL,
	PRIMARY KEY(smt_pis_id),
	CONSTRAINT smt_pis_pis_fkey FOREIGN KEY(scene_id, person_id) REFERENCES person_in_scene (scene_id, person_id),
	CONSTRAINT smt_pis_submodal_fkey FOREIGN KEY(submodaltype_name, modaltype_name) REFERENCES submodal_type (submodaltype_name, modaltype_name)
);

CREATE TABLE event_data(
	event_data_id SERIAL,
	event_time TIME NOT NULL,
	PRIMARY KEY(event_data_id)
);

CREATE TABLE interval_data(
	interval_data_id SERIAL,
	start_time TIME NOT NULL,
	end_time TIME NOT NULL,
	PRIMARY KEY(interval_data_id)
);

CREATE TABLE represent_type(
	represent_type_id SERIAL,
	smt_pis_id INTEGER NOT NULL,
	value DOUBLE PRECISION,
	subtitle VARCHAR(100),

	event_data_id INTEGER,
	interval_data_id INTEGER,
	index_data INTEGER,
	PRIMARY KEY(represent_type_id),
	CONSTRAINT represent_type_smt_pis_fkey FOREIGN KEY(smt_pis_id) REFERENCES smt_pis (smt_pis_id),

	CONSTRAINT represent_type_event_data_id_fkey FOREIGN KEY(event_data_id) REFERENCES event_data (event_data_id),
	CONSTRAINT represent_type_interval_data_id_fkey FOREIGN KEY(interval_data_id) REFERENCES interval_data (interval_data_id),
	
	CONSTRAINT represent_type_event_unique UNIQUE(event_data_id),
	CONSTRAINT represent_type_interval_unique UNIQUE(interval_data_id),
	CONSTRAINT represent_type_timeless_unique UNIQUE(represent_type_id, index_data)
);