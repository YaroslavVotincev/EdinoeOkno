CREATE schema forms;
set search_path = forms;

CREATE TABLE forms
(
	id_form serial NOT NULL,
	name_form text NOT NULL,
	description text NOT NULL,
	html_code text,
	PRIMARY KEY (id_form)	
);

CREATE TABLE questions
(
	id_question serial NOT NULL,
	id_form INT NOT NULL,
	name_question text NOT NULL,
	type_question text NOT NULL,
	is_required bool NOT NULL DEFAULT FALSE,
	PRIMARY KEY (id_question),
	FOREIGN KEY (id_form)
		REFERENCES forms ( id_form )
		ON DELETE CASCADE
);

CREATE TABLE answers
(
	id_answer serial NOT NULL,
	id_question INT NOT NULL,
	name_answer text NOT NULL,
	is_text_input bool NOT NULL DEFAULT FALSE,
	max_text_input int DEFAULT NULL,
	PRIMARY KEY (id_answer),
	FOREIGN KEY (id_question)
		REFERENCES questions ( id_question )
		ON DELETE CASCADE
);

CREATE TABLE response
(
	id_response serial NOT NULL,
	respondent text NOT NULL,
	id_question INT,
	id_answer INT,
	text_input text,
	PRIMARY KEY (id_response, respondent),	
	FOREIGN KEY (id_question)
		REFERENCES questions ( id_question )
		ON DELETE CASCADE,
	FOREIGN KEY (id_answer)
		REFERENCES answers ( id_answer )
		ON DELETE CASCADE
);

CREATE VIEW stats_v AS
(
	WITH answer_count AS
	(
		SELECT a.id_question, a.id_answer, a.name_answer, a.is_text_input, count(a.id_answer) as counted_answers
			FROM forms.answers a
			JOIN forms.response r ON r.id_answer = a.id_answer
			GROUP BY 1,2
		UNION  
		SELECT id_question, id_answer, name_answer, is_text_input, 0 FROM forms.answers
			WHERE id_answer NOT IN (SELECT id_answer FROM response)
	)
	SELECT f.id_form, q.id_question, 
		q.name_question, q.type_question, 
		ac.id_answer, ac.name_answer, 
		ac.counted_answers, ac.is_text_input
	FROM answer_count ac
	JOIN forms.questions q ON q.id_question = ac.id_question
	JOIN forms.forms f ON f.id_form = q.id_form
	ORDER BY 1,2,5
);

CREATE VIEW text_inputs_v AS
(
	SELECT a.id_answer, string_agg(r.text_input, E'\t' order by r.text_input) as text_answers
	FROM forms.response r
	JOIN forms.answers a ON r.id_answer = a.id_answer
	GROUP BY 1
	HAVING a.is_text_input = true
	ORDER BY 1
);
/*
INSERT INTO forms(name_form, description) 
VALUES ('Тестовая анкета','test');
INSERT INTO questions(id_form,name_question,type_question) 
VALUES
(1, 'Вопрос1', 'text_input'),
(1, 'Вопрос2', 'checkbox'),
(1, 'Вопрос3', 'checkbox_with_text_input'),
(1, 'Вопрос4', 'radio'),
(1, 'Вопрос5', 'radio_with_text_input');

INSERT INTO answers(id_question,name_answer,is_text_input,max_text_input) 
VALUES
(1, 'Ответ', true, 20),
(2, 'Вариант 1', false, NULL),
(2, 'Вариант 2', false, NULL),
(2, 'Вариант 3', false, NULL),
(3, 'Вариант 1', false, NULL),
(3, 'Другое', true, 20),
(4, 'Вариант 1', false, NULL),
(4, 'Вариант 2', false, NULL),
(5, 'Вариант 1', false, NULL),
(5, 'Другое', true, 20);

INSERT INTO response(respondent,id_question,id_answer)
VALUES
('1',1,1),
('1',2,2),
('1',3,5),
('1',4,7),
('1',5,10)
*/
--SELECT * from forms;
--SELECT * from questions;
--SELECT * from answers;
--SELECT * from response;