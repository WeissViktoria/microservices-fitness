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

