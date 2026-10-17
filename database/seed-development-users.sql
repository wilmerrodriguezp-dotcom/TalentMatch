USE TalentMatchDb;

INSERT IGNORE INTO Users (UserName, PasswordHash, Role)
VALUES
    ('admin', 'PBKDF2-SHA256$600000$iRnMWXs6hphDvC1XncnGzg==$15ef/40gLfGdrqcZRLMKH1FKcXxLHDwdMJJ4977J8uM=', 'Admin'),
    ('user', 'PBKDF2-SHA256$600000$KMyWz34zTf8hWJhwedsn9A==$9eAtL5tBNMKgQVNQAkXdh6R238tUpIJqzSesN1lzfZk=', 'User');
