-- Revert admin:tasks from sqlite

BEGIN;

DROP TABLE IF EXISTS tasks;

COMMIT;
