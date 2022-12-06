CREATE schema forms;
set search_path = forms;

CREATE TABLE forms
(
	id_form serial NOT NULL,
	name_form text NOT NULL,
	description text NOT NULL,
	PRIMARY KEY (id_form)	
);

CREATE TABLE questions
(
	id_question serial NOT NULL,
	id_form INT NOT NULL,
	name_question text NOT NULL,
	type_question text NOT NULL,
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
	id_form INT,
	id_question INT,
	id_answer INT,
	text_input text;
	PRIMARY KEY (id_response, id_respondent),	
	FOREIGN KEY (id_form)
		REFERENCES forms ( id_form )
		ON DELETE CASCADE,
	FOREIGN KEY (id_question)
		REFERENCES questions ( id_question )
		ON DELETE CASCADE,
	FOREIGN KEY (id_answer)
		REFERENCES answers ( id_answer )
		ON DELETE CASCADE,
)