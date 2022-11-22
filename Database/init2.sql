CREATE SCHEMA dev1;
SET search_path = dev1;

CREATE TABLE requests
(
	request_id serial,
	request_code varchar(3),
	faculty_code varchar(3),
	status_code varchar(3) DEFAULT '100',
	response_id int DEFAULT 0,
	doc_storage_id int DEFAULT 0,
	
	first_name varchar(20) NOT NULL DEFAULT '',
	last_name varchar(20) NOT NULL DEFAULT '',
	patronymic varchar(20) NOT NULL DEFAULT '',
	email text NOT NULL,
	student_group varchar(10) NOT NULL,
	
	time_when_added TIMESTAMP DEFAULT now(),
	time_when_updated TIMESTAMP DEFAULT now(),
	
	PRIMARY KEY (request_id)
);

CREATE TABLE request_codes
(
	request_code varchar(3),
	request_name text NOT NULL,
	PRIMARY KEY(request_code)
);

CREATE TABLE faculty_codes
(
	faculty_code varchar(3),
	
	faculty_name text NOT NULL,
	faculty_short_name varchar(10) NOT NULL,
	
	UNIQUE (faculty_short_name),
	PRIMARY KEY (faculty_code)
);

CREATE TABLE status_codes
(
	status_code varchar(3),
	status_name text NOT NULL,
	PRIMARY KEY (status_code)
);

CREATE TABLE questions
(
	question_id serial,
	status_code varchar(3),
	faculty_code varchar(3),
	response_id int DEFAULT 0,
	
	first_name varchar(20) NOT NULL DEFAULT '',
	last_name varchar(20) NOT NULL DEFAULT '',
	patronymic varchar(20) NOT NULL DEFAULT '',
	email text NOT NULL,
	student_group varchar(10) NOT NULL,
	
	subject varchar(20) NOT NULL,
	question text NOT NULL,
	
	time_when_added TIMESTAMP DEFAULT now(),
	time_when_updated TIMESTAMP DEFAULT now(),
	
	CONSTRAINT email_validity_check CHECK (email ~ '^[a-zA-Z0-9.!#$%&''*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$'),
	CONSTRAINT group_validity_check CHECK (student_group ~ '^(Б|С|М|б|с|м)\d\d-\d\d\d-\d$'),
	PRIMARY KEY (question_id)
);

CREATE TABLE staff_request_privileges
(
	privilege_id serial,
	login varchar(10) NOT NULL,
	request_code varchar(3) NOT NULL,
	
	UNIQUE (login,request_code),
	PRIMARY KEY(privilege_id)
);

CREATE TABLE staff_members
(
	login varchar(10),
	faculty_code varchar(3) DEFAULT '000',
	
	password varchar(20) NOT NULL DEFAULT '',
	
	first_name varchar(20) NOT NULL DEFAULT '',
	last_name varchar(20) NOT NULL DEFAULT '',
	patronymic varchar(20) NOT NULL DEFAULT '',
	
	can_view_questions bool NOT NULL DEFAULT true,
	can_view_appointments bool NOT NULL DEFAULT true,
	can_view_forms bool NOT NULL DEFAULT true,
	
	PRIMARY KEY(login)
);

CREATE TABLE responses
(
	response_id serial,
	staff_member_login varchar(10) DEFAULT 'admin',
	
	email text NOT NULL,
	title text NOT NULL,
	content text NOT NULL,
	type text NOT NULL,
	time_sended TIMESTAMP DEFAULT now(),
	
	PRIMARY KEY (response_id)
);

CREATE TABLE doc_storage
(
	doc_storage_id serial,
	
	doc_amount int NOT NULL DEFAULT 0,
	dir_name text NOT NULL DEFAULT '',
	public_url text NOT NULL DEFAULT 'https://yadi.sk/',
	
	PRIMARY KEY (doc_storage_id)
);

CREATE TABLE req_front
(
	request_code varchar(3) REFERENCES request_codes,
	first_name varchar(20) NOT NULL,
	last_name varchar(20) NOT NULL,
	patronymic varchar(20) NOT NULL DEFAULT '',
	email text NOT NULL,
	faculty_code varchar(3) REFERENCES faculty_codes,
	student_group varchar(10) NOT NULL,
	doc_amount int NOT NULL DEFAULT 0,
	dir_name text NOT NULL DEFAULT '',
	public_url text NOT NULL DEFAULT '',
	
	CONSTRAINT email_validity_check CHECK (email ~ '^[a-zA-Z0-9.!#$%&''*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$'),
	CONSTRAINT group_validity_check CHECK (student_group ~ '^(Б|С|М|б|с|м)\d\d-\d\d\d-\d$'),
	CONSTRAINT attachements_validity_check CHECK (doc_amount = 0 OR public_url LIKE 'https://yadi.sk/d/%' AND dir_name ~ '^[A-Za-z0-9]+$')
);

ALTER TABLE requests ADD FOREIGN KEY (request_code) REFERENCES request_codes ON DELETE RESTRICT ON UPDATE CASCADE;
ALTER TABLE requests ADD FOREIGN KEY (status_code) REFERENCES status_codes ON DELETE RESTRICT ON UPDATE CASCADE;
ALTER TABLE requests ADD FOREIGN KEY (faculty_code) REFERENCES faculty_codes ON DELETE RESTRICT ON UPDATE CASCADE;
ALTER TABLE requests ADD FOREIGN KEY (response_id) REFERENCES responses ON DELETE SET DEFAULT;
ALTER TABLE requests ADD FOREIGN KEY (doc_storage_id) REFERENCES doc_storage ON DELETE SET DEFAULT;

ALTER TABLE questions ADD FOREIGN KEY (status_code) REFERENCES status_codes ON DELETE RESTRICT ON UPDATE CASCADE;
ALTER TABLE questions ADD FOREIGN KEY (faculty_code) REFERENCES faculty_codes ON DELETE RESTRICT ON UPDATE CASCADE;
ALTER TABLE questions ADD FOREIGN KEY (response_id) REFERENCES responses ON DELETE SET DEFAULT;

ALTER TABLE staff_members ADD FOREIGN KEY (faculty_code) REFERENCES faculty_codes ON DELETE RESTRICT ON UPDATE CASCADE;

ALTER TABLE staff_request_privileges ADD FOREIGN KEY (login) REFERENCES staff_members ON DELETE CASCADE ON UPDATE CASCADE;
ALTER TABLE staff_request_privileges ADD FOREIGN KEY (request_code) REFERENCES request_codes ON DELETE CASCADE ON UPDATE CASCADE;

ALTER TABLE responses ADD FOREIGN KEY (staff_member_login) REFERENCES staff_members ON DELETE SET DEFAULT ON UPDATE CASCADE;

CREATE INDEX request_last_name_index ON requests(last_name);
CREATE INDEX request_email_index ON requests(email);
CREATE INDEX request_time_when_updated_index ON requests(time_when_updated);
CREATE INDEX request_request_code_index ON requests(request_code);
CREATE INDEX request_faculty_code_index ON requests(faculty_code);
CREATE INDEX request_student_group_index ON requests(student_group);

CREATE INDEX questions_last_name_index ON questions(last_name);
CREATE INDEX questions_email_index ON questions(email);
CREATE INDEX questions_time_when_updated_index ON questions(time_when_updated);
CREATE INDEX questions_faculty_code_index ON questions(faculty_code);
CREATE INDEX questions_student_group_index ON questions(student_group);

CREATE INDEX staff_last_name_index ON requests(last_name);
CREATE INDEX staff_faculty_code_index ON staff_members(faculty_code);

CREATE FUNCTION req_front_insert() RETURNS trigger AS $req_front_insert$
BEGIN
	IF NEW.doc_amount = 0 THEN
		INSERT INTO requests(request_code, first_name, last_name, patronymic, email, faculty_code, student_group)
		VALUES (NEW.request_code, NEW.first_name, NEW.last_name, NEW.patronymic, NEW.email, NEW.faculty_code, NEW.student_group);
	ELSE
		WITH tmp AS
		(INSERT INTO doc_storage (doc_amount, dir_name, public_url) 
		 VALUES (NEW.doc_amount, NEW.dir_name, NEW.public_url)
		 RETURNING doc_storage_id
		)
		INSERT INTO requests(request_code, first_name, last_name, patronymic, email, faculty_code, student_group, doc_storage_id)
			SELECT NEW.request_code, NEW.first_name, NEW.last_name, NEW.patronymic, 
				NEW.email, NEW.faculty_code, NEW.student_group, tmp.doc_storage_id
			FROM tmp;
	END IF;
	RETURN NULL;
END;
$req_front_insert$ LANGUAGE plpgsql;

CREATE TRIGGER req_front_insert
BEFORE INSERT ON req_front
    FOR EACH ROW EXECUTE FUNCTION req_front_insert();
	
CREATE FUNCTION request_delete() RETURNS trigger AS $request_delete$	
BEGIN
	IF OLD.response_id <> 0 THEN
		DELETE FROM responses WHERE response_id = OLD.response_id;
	END IF;
	IF OLD.doc_storage_id <> 0 THEN
		DELETE FROM doc_storage WHERE doc_storage_id = OLD.doc_storage_id;
	END IF;
	RETURN NULL;
END;
$request_delete$ LANGUAGE plpgsql;

CREATE TRIGGER request_delete
BEFORE DELETE ON requests
    FOR EACH ROW EXECUTE FUNCTION request_delete();
/*
CREATE FUNCTION doc_storage_delete() RETURNS trigger AS 
$doc_storage_delete$

	import requests
	dir_name = TD["old"]["dir_name"];
	json_headers = {
		"Authorization": "OAuth y0_AgAAAABl4J01AAiOLAAAAADTIhvZdGfIs24sRhqhWMv8Wa6WzOVu6TQ"
	}
	params = {
		"path": "uploads/%s" %dir_name
	}
	requests.delete("https://cloud-api.yandex.net/v1/disk/resources", params=params, headers=json_headers)
	
$doc_storage_delete$ 
LANGUAGE plpython3u;

CREATE TRIGGER doc_storage_delete
BEFORE DELETE ON doc_storage
    FOR EACH ROW EXECUTE FUNCTION doc_storage_delete();
*/

INSERT INTO request_codes (request_code, request_name)
VALUES 
('000', 'Справка о факте обучения (по месту требования)'),
('001', 'Справка о факте обучения (в органы социальной защиты, пенсионный фонд)'),
('002', 'Справка-вызов'),
('003', 'Копия договора на обучение'),
('P00', 'Изменение персональных данных'),
('P01', 'Предоставление или продление академического отпуска'),
('P02', 'Предоставление отпуска по беременности и родам'),
('P03', 'Предоставление отпуска по уходу за ребенком до 3 лет'),
('P04', 'Заявление об отчислении'),
('P05', 'Заявление о переводе внутри университета'),
('P06', 'Заявление о восстановлении');

INSERT INTO faculty_codes (faculty_code, faculty_name, faculty_short_name)
VALUES
('000', 'Институт «Современные технологии машиностроения, автомобилестроения и металлургии»', 'СТМАиМ'),
('001', 'Теплотехнический факультет', 'ТТ'),
('002', 'Машиностроительный факультет', 'М'),
('003', 'Факультет «Математики и Естественных Наук»', 'МиЕН'),
('004', 'Приборостроительный факультет', 'П'),
('005', 'Факультет «Информатика и вычислительная техника»', 'ИВТ'),
('006', 'Инженерно-экономический факультет', 'ИЭ'),
('007', 'Инженерно-строительный факультет', 'ИС'),
('008', 'Факультет «Право и гуманитарные науки»', 'ПиГН'),
('009', 'Факультет «Управление качеством»', 'УК'),
('010', 'Факультет «Реклама и дизайн»', 'РиД'),
('011', 'Институт дополнительного профессионального образования', 'ИДПО'),
('012', 'Иниститут непрерывного профессионального образования ИжГТУ', 'ИНПО'),
('013', 'Институт физической культуры и спорта', 'ИФКиС'),
('014', 'Институт экономики, управления и финансов', 'ИЭУФ'),
('015', 'Управление магистратуры, аспирантуры и докторантуры', 'УМАД');

INSERT INTO status_codes (status_code, status_name)
VALUES
('100', 'Новый запрос'),
('101', 'Запрос выполнен'),
('102', 'Запрос отклонен');

INSERT INTO staff_members (login, password, first_name, last_name, patronymic)
VALUES
('admin', 'admin', '1','1','1');

INSERT INTO responses (response_id, email, title, content, type)
VALUES
(0,'edinoeokno@internet.ru','Без названия','Ответ не добавлен','default_no_response');

INSERT INTO doc_storage (doc_storage_id)
VALUES
(0);
/*
CREATE VIEW req_new_back AS 
(
SELECT 
	r.request_id, 
	rc.request_name,
	sc.status_name, 
	r.first_name, 
	r.last_name,
	r.patronymic,
	r.email,
	fc.faculty_name, 
	fc.short_name,
	r.student_group,
	r.dir_path,
	r.files_attached, 
	to_char(r.time_when_added, 'hh24:mi dd/mm/yyyy') as time_when_added
FROM requests r, request_codes rc, status_codes sc, faculty_codes fc
WHERE rc.request_code = r.request_code
AND fc.faculty_code = r.faculty_code
AND sc.status_code = r.status_code
AND r.status_code = '100'
ORDER BY r.time_when_added DESC
);

CREATE VIEW req_done_back AS 
(
SELECT 
	r.request_id, 
	rc.request_name,
	sc.status_name, 
	r.first_name, 
	r.last_name,
	r.patronymic,
	r.email,
	fc.faculty_name, 
	fc.short_name,
	r.student_group,
	r.dir_path,
	r.files_attached, 
	to_char(r.time_when_added, 'hh24:mi dd/mm/yyyy') as time_when_added,
	r.email_response,
	to_char(r.time_when_update, 'hh24:mi dd/mm/yyyy') as time_when_update
FROM requests r, request_codes rc, status_codes sc, faculty_codes fc
WHERE rc.request_code = r.request_code
AND fc.faculty_code = r.faculty_code
AND sc.status_code = r.status_code
AND r.status_code = '101'
ORDER BY r.time_when_added DESC
);

CREATE VIEW req_decl_back AS 
(
SELECT 
	r.request_id, 
	rc.request_name,
	sc.status_name, 
	r.first_name, 
	r.last_name,
	r.patronymic,
	r.email,
	fc.faculty_name, 
	fc.short_name,
	r.student_group,
	r.dir_path,
	r.files_attached, 
	to_char(r.time_when_added, 'hh24:mi dd/mm/yyyy') as time_when_added,
	r.email_response,
	to_char(r.time_when_update, 'hh24:mi dd/mm/yyyy') as time_when_update
FROM requests r, request_codes rc, status_codes sc, faculty_codes fc
WHERE rc.request_code = r.request_code
AND fc.faculty_code = r.faculty_code
AND sc.status_code = r.status_code
AND r.status_code = '102'
ORDER BY r.time_when_added DESC
);
*/
/*
select * from doc_storage;
select * from responses;
select * from req_front;
select * from requests;
*/
/*
insert into req_front (request_code, first_name, last_name, patronymic, email, faculty_code, student_group) 
values
('000','Pasha','Pavlov','Pavlovich','123@gmail.com','000','Б20-191-1');

insert into req_front (request_code, first_name, last_name, patronymic, email, faculty_code, student_group, doc_amount, dir_name, public_url) 
values
('000','Pasha','Pavlov','Pavlovich','123@gmail.com','000','Б20-191-1', 1, 'test1', 'https://yadi.sk/d/IOSGdMqTQ7zBcw');
*/
--TRUNCATE requests;
