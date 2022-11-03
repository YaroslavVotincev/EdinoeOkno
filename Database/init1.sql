CREATE SCHEMA dev;
SET search_path = dev;
CREATE TABLE requests
(
	request_id serial,
	request_code varchar(3) NOT NULL,
	status_code varchar(3) NOT NULL DEFAULT '100',
	first_name varchar(20) NOT NULL,
	last_name varchar(20) NOT NULL,
	patronymic varchar(20),
	email text NOT NULL,
	faculty_code varchar(3) NOT NULL,
	student_group varchar(10) NOT NULL,
	dir_path varchar DEFAULT 'Без прикреплённых файлов',
	files_attached int DEFAULT 0,
	time_when_added TIMESTAMP DEFAULT current_timestamp,
	email_response text DEFAULT 'Ответ не добавлен',
	time_when_update TIMESTAMP DEFAULT current_timestamp,
	PRIMARY KEY (request_id),
	CONSTRAINT email_validity_check CHECK 
		(email ~ '^[a-zA-Z0-9.!#$%&''*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$')
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
	short_name varchar(10) NOT NULL,
	UNIQUE (short_name),
	PRIMARY KEY (faculty_code)
);
CREATE TABLE consultation_schedule
(
	schedule_id varchar(3),
	day_of_the_week text NOT NULL,
	time_interval text NOT NULL,
	info text DEFAULT 'Без информации',
	PRIMARY KEY (schedule_id)
);
CREATE TABLE consultation_appointment
(
	appointment_id serial,
	status_code varchar(3),
	first_name varchar(20) NOT NULL,
	last_name varchar(20) NOT NULL,
	patronymic varchar(20),
	email text NOT NULL,
	subject text NOT NULL,
	appointment_time_code varchar(3) NOT NULL,
	time_when_added TIMESTAMP DEFAULT current_timestamp,
	email_response text DEFAULT 'Ответ не добавлен',
	time_when_update TIMESTAMP DEFAULT current_timestamp,
	PRIMARY KEY (appointment_id)
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
	email text NOT NULL,
	first_name varchar(20) NOT NULL,
	last_name varchar(20) NOT NULL,
	patronymic varchar(20),
	student_group varchar(10) NOT NULL,
	question text NOT NULL,
	time_when_added TIMESTAMP DEFAULT current_timestamp,
	email_response text DEFAULT 'Ответ не добавлен',
	time_when_update TIMESTAMP DEFAULT current_timestamp,
	PRIMARY KEY (question_id)
);
CREATE TABLE staff_request_privileges
(
	privilege_id varchar(3),
	login varchar(10) NOT NULL,
	request_code varchar(3) NOT NULL,
	UNIQUE (login,request_code),
	PRIMARY KEY(privilege_id)
);
CREATE TABLE staff_members
(
	login varchar(10),
	password varchar(20) NOT NULL,
	faculty_code varchar(3) NOT NULL,
	first_name varchar(20) NOT NULL,
	last_name varchar(20) NOT NULL,
	patronymic varchar(20),
	can_view_questions bool DEFAULT false,
	can_view_appointments bool DEFAULT false,
	PRIMARY KEY(login)
);

ALTER TABLE requests ADD FOREIGN KEY (request_code) REFERENCES request_codes;
ALTER TABLE requests ADD FOREIGN KEY (status_code) REFERENCES status_codes;
ALTER TABLE requests ADD FOREIGN KEY (faculty_code) REFERENCES faculty_codes;

ALTER TABLE staff_request_privileges ADD FOREIGN KEY (login) REFERENCES staff_members;
ALTER TABLE staff_request_privileges ADD FOREIGN KEY (request_code) REFERENCES request_codes;

ALTER TABLE staff_members ADD FOREIGN KEY (faculty_code) REFERENCES faculty_codes;

ALTER TABLE questions ADD FOREIGN KEY (status_code) REFERENCES status_codes;

ALTER TABLE consultation_appointment ADD FOREIGN KEY (status_code) REFERENCES status_codes;
ALTER TABLE consultation_appointment ADD FOREIGN KEY (appointment_time_code) REFERENCES consultation_schedule;

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

INSERT INTO faculty_codes (faculty_code, faculty_name, short_name)
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
('100', 'New request'),
('101', 'Request done'),
('102', 'Request declined');

CREATE VIEW req_front AS 
(SELECT 
 	r.request_code,
 	r.first_name,
	r.last_name,
	r.patronymic,
	r.email,
	r.faculty_code,
	r.student_group,
	r.dir_path,
	r.files_attached 
FROM requests r);

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
FROM dev.requests r, dev.request_codes rc, dev.status_codes sc, dev.faculty_codes fc
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
FROM dev.requests r, dev.request_codes rc, dev.status_codes sc, dev.faculty_codes fc
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
FROM dev.requests r, dev.request_codes rc, dev.status_codes sc, dev.faculty_codes fc
WHERE rc.request_code = r.request_code
AND fc.faculty_code = r.faculty_code
AND sc.status_code = r.status_code
AND r.status_code = '102'
ORDER BY r.time_when_added DESC
);

insert into dev.req_front values
('000','Pasha','Pavlov','Pavlovich','123@gmail.com','000','B20-191','',0);
INSERT INTO dev.req_front VALUES 
('002','Andrey','Bolshoi','Ivanovich','vasya@mail.ru','007','G19-192','',2);
INSERT INTO dev.req_front VALUES 
('003','Artem','Ciklop','Petrov','artem@mail.ru','004','R1-12','',1);
INSERT INTO dev.req_front VALUES 
('002','Vasiliy','Zabegaev','Artemovich','lupa@mail.ru','003','G19-192','',2);
INSERT INTO dev.req_front VALUES 
('002','Petya','Vasnecov','Vasilyevich','pupa@mail.ru','001','H19-192','',2);
INSERT INTO dev.req_front VALUES 
('002','Andrey','Bolshoi','Ivanovich','vasya@mail.ru','007','G19-192','',2);
INSERT INTO dev.req_front VALUES 
('P04','Zhenya','Makarov','Danilovich','1234@mail.ru','002','G19-182','',1);
INSERT INTO dev.req_front VALUES 
('P02','Rustam','Bibivoch','Ivanovich','5555@mail.ru','004','G19-192','',2);
INSERT INTO dev.req_front VALUES 
('002','Gleb','Smit','Ivanovich','twerk@mail.ru','015','G19-192','',2);
--DELETE from requests;
SELECT * from req_new_back;
--SELECT * from req_done_back;
--SELECT * from req_decl_back;
