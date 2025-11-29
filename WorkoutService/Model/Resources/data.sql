CREATE DATABASE IF NOT EXISTS WorkoutDB;
CREATE DATABASE IF NOT EXISTS AnalyticsDB;
CREATE DATABASE IF NOT EXISTS RecommendationDB;

-- Workout data ----------------------------------------------------
USE WorkoutDB;

CREATE TABLE IF NOT EXISTS Workouts (
    WorkoutId      INT AUTO_INCREMENT PRIMARY KEY,
    ParticipantId  INT NOT NULL,
    ExerciseType   VARCHAR(100) NOT NULL,
    Duration       INT NOT NULL,
    Repetitions    INT NOT NULL,
    Intensity      VARCHAR(50) NOT NULL,
    Date           DATETIME NOT NULL,
    CaloriesBurned INT NOT NULL,
    CreatedAt      DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt      DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

INSERT INTO Workouts (ParticipantId, ExerciseType, Duration, Repetitions, Intensity, Date, CaloriesBurned)
VALUES
    (1, 'HIIT Session', 35, 20, 'Hoch',  DATE_SUB(NOW(), INTERVAL 2 DAY), 420),
    (1, 'Krafttraining Push', 50, 12, 'Mittel', DATE_SUB(NOW(), INTERVAL 4 DAY), 360),
    (1, 'Yoga Mobility', 40, 0, 'Niedrig', DATE_SUB(NOW(), INTERVAL 6 DAY), 190),
    (2, '5km Lauf', 30, 0, 'Hoch', DATE_SUB(NOW(), INTERVAL 1 DAY), 330);

-- Analytics history ------------------------------------------------
USE AnalyticsDB;

CREATE TABLE IF NOT EXISTS History (
                                       AnalyticsId INT AUTO_INCREMENT PRIMARY KEY,
                                       ParticipantId INT NOT NULL,
                                       PeriodStart DATETIME NOT NULL,
                                       PeriodEnd DATETIME NOT NULL,
                                       TotalWorkouts INT NOT NULL,
                                       AverageDuration DOUBLE NOT NULL,
                                       TotalCalories DOUBLE NOT NULL,
                                       BestPerformance VARCHAR(255),
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
    );



CREATE DATABASE IF NOT EXISTS RecommendationDB;
USE RecommendationDB;
CREATE TABLE IF NOT EXISTS Recommendations (
                                               RecommendationId INT AUTO_INCREMENT PRIMARY KEY,
                                               ParticipantId INT NOT NULL,
                                               RecommendationType VARCHAR(50) NOT NULL,
    Description VARCHAR(500) NOT NULL,
    DateGenerated DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
    );

INSERT INTO Recommendations (ParticipantId, RecommendationType, Description)
VALUES
    (101, 'Health', 'Encourage daily 30-minute walks to improve cardiovascular health.'),
    (102, 'Career', 'Consider attending a project management workshop to enhance leadership skills.'),
    (103, 'Education', 'Enroll in an online data analytics course to strengthen analytical abilities.'),
    (104, 'Wellness', 'Practice mindfulness meditation for 10 minutes each morning.'),
    (105, 'Fitness', 'Increase weekly strength training sessions from two to three.'),
    (106, 'Nutrition', 'Incorporate more plant-based meals into weekly diet.'),
    (107, 'Financial', 'Start budgeting monthly expenses using a tracking app.'),
    (108, 'Social', 'Participate in local community events to build social connections.'),
    (109, 'Productivity', 'Adopt the Pomodoro technique to manage work sessions more effectively.'),
    (110, 'Lifestyle', 'Develop a consistent sleep schedule targeting at least 7 hours nightly.');
