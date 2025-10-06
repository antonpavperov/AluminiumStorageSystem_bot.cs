-- Пример заполнения базовых данных
-- -- Основные типы лома (верхний уровень)
INSERT INTO scrap_types (name, description) 
VALUES 
 ('Банка БУ', 'Бывшие в употреблении алюминиевые банки без железа'),
 ('Электротехнический лом', 'Электротехнические изделия из алюминия'),
 ('Пищевой алюминий', 'Пищевой алюминий'),
 ('Ад31','Профиль'),
 ('АМг', 'Лом сплавов АМг'),
 ('Лом II-IV гр', 'Листы трубы высечка');

 INSERT INTO scrap_types (name, parent_id, description) 
VALUES 
 ('Банка б/у россыпь', (SELECT id FROM scrap_types WHERE name = 'Банка БУ'), 'Бывшие в употреблении алюминиевые банки без железа'),
 ('Банка б/у брикет', (SELECT id FROM scrap_types WHERE name = 'Банка БУ'), 'Бывшие в употреблении алюминиевые банки без железа');

 INSERT INTO scrap_types (name, parent_id, description) 
VALUES 
 ('Лом эл-тех', (SELECT id FROM scrap_types WHERE name = 'Электротехнический лом'), 'Эл-тех не требующий доработки'),
 ('Лом электротехнических изделий', (SELECT id FROM scrap_types WHERE name = 'Электротехнический лом'), 'Требующий доработки');

 INSERT INTO scrap_types (name, parent_id, description) 
VALUES 
 ('Пищевой алюминий', (SELECT id FROM scrap_types WHERE name = 'Пищевой алюминий'), 'Пищевой алюминий');

  INSERT INTO scrap_types (name, parent_id, description) 
VALUES 
 ('Профиль чистый', (SELECT id FROM scrap_types WHERE name = 'Ад31'), 'Отходы заводские чистые'),
 ('Профиль с термовставкми', (SELECT id FROM scrap_types WHERE name = 'Ад31'), 'С  термовставкми');

INSERT INTO scrap_types (name, parent_id, description) 
VALUES 
 ('АМг2', (SELECT id FROM scrap_types WHERE name = 'АМг'), 'Листы трубы высечка без приделок'),
 ('АМг3', (SELECT id FROM scrap_types WHERE name = 'АМг'), 'Листы трубы высечка без приделок'),
 ('АМг4, АМг5, АМг6', (SELECT id FROM scrap_types WHERE name = 'АМг'), 'Листы трубы высечка без приделок'),
 ('Лома сплавов АМг микс', (SELECT id FROM scrap_types WHERE name = 'АМг'), 'Листы трубы высечка без приделок');

  INSERT INTO scrap_types (name, parent_id, description) 
VALUES 
 ('Лом II-IV гр', (SELECT id FROM scrap_types WHERE name = 'Лом II-IV гр'), 'Листы трубы высечка Д16, АМг, АД31'),
 ('Лом II-IV гр пресс пакет', (SELECT id FROM scrap_types WHERE name = 'Лом II-IV гр'), 'Листы трубы высечка Д16, АМг, АД31');
-- Начальные данные для менеджеров (пример)
INSERT INTO managers (name, phone, email, position) 
VALUES 
('Антон Павперов', '+79992122672', 'me@pavperov.ru', 'Менеджер ОЦМ'),
('Михаил Павперов', '+79219413123', 'mixail67@gmail.com', 'Менеджер ОЦМ');
