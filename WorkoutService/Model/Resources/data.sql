CREATE DATABASE WorkoutDB;
 CREATE DATABASE IF NOT EXISTS AnalyticsDB;
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



CREATE DATABASE RecommendationDB;
