-- Deploy admin:tasks to sqlite

BEGIN;

CREATE TABLE IF NOT EXISTS tasks
(
    id        INTEGER PRIMARY KEY AUTOINCREMENT,
    name      TEXT     NOT NULL,
    data      BLOB     NOT NULL
);

COMMIT;
