-- Verify admin:tasks on sqlite

BEGIN;

SELECT id, name, data
    FROM tasks;

ROLLBACK;
