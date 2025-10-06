-- Создание таблицы менеджеров
CREATE TABLE IF NOT EXISTS managers (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    phone VARCHAR(20),
    email VARCHAR(100),
    position VARCHAR(50)
);

-- Создание таблицы поставщиков
CREATE TABLE IF NOT EXISTS suppliers (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    region VARCHAR(100) NOT NULL,
    manager_id INT REFERENCES managers(id)
);

-- Создание таблицы типов лома
CREATE TABLE IF NOT EXISTS scrap_types (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    parent_id INT REFERENCES scrap_types(id),
    description TEXT
);

-- Создание таблицы заявок
CREATE TABLE IF NOT EXISTS applications (
    id SERIAL PRIMARY KEY,
    supplier_id INT REFERENCES suppliers(id),
    manager_id INT REFERENCES managers(id),
    application_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    status VARCHAR(20) DEFAULT 'Новая',
    comment TEXT
);

-- Создание таблицы позиций заявок
CREATE TABLE IF NOT EXISTS application_items (
    id SERIAL PRIMARY KEY,
    application_id INT REFERENCES applications(id),
    scrap_type_id INT REFERENCES scrap_types(id),
    quantity DECIMAL(10,2) NOT NULL,
    price DECIMAL(10,2),
    total_price DECIMAL(10,2)
);
-- Создание индексов для оптимизации
CREATE INDEX idx_managers_name ON managers(name);
CREATE INDEX idx_suppliers_name ON suppliers(name);
CREATE INDEX idx_scrap_types_name ON scrap_types(name);
CREATE INDEX idx_applications_supplier ON applications(supplier_id);
CREATE INDEX idx_applications_manager ON applications(manager_id);
CREATE INDEX idx_application_items_application ON application_items(application_id);
CREATE INDEX idx_application_items_scrap_type ON application_items(scrap_type_id);
