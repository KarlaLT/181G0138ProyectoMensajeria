CREATE DATABASE  IF NOT EXISTS `mensajeriatecnm` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `mensajeriatecnm`;
-- MySQL dump 10.13  Distrib 8.0.29, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: mensajeriatecnm
-- ------------------------------------------------------
-- Server version	8.0.29

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `carreras`
--

DROP TABLE IF EXISTS `carreras`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `carreras` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(200) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `carreras`
--

LOCK TABLES `carreras` WRITE;
/*!40000 ALTER TABLE `carreras` DISABLE KEYS */;
INSERT INTO `carreras` VALUES (1,'Sistemas computacionales'),(2,'Mecatrónica'),(3,'Industrial'),(4,'Petrolera'),(5,'Electromecánica'),(6,'Administración');
/*!40000 ALTER TABLE `carreras` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `clases`
--

DROP TABLE IF EXISTS `clases`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `clases` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(200) NOT NULL,
  `IdGrupo` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_clase_grupo_idx` (`IdGrupo`),
  CONSTRAINT `fk_clase_grupo` FOREIGN KEY (`IdGrupo`) REFERENCES `grupos` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clases`
--

LOCK TABLES `clases` WRITE;
/*!40000 ALTER TABLE `clases` DISABLE KEYS */;
INSERT INTO `clases` VALUES (1,'Integración de proyectos de software',1),(2,'Inteligencia Artificial',1),(3,'Integración de proyectos de software',2),(4,'Inteligencia Artificial',2),(5,'Aplicaciones cliente-servidor',3),(6,'Taller de investigación II',3),(7,'Cálculo integral',4),(8,'Metodología de la investigación II',4),(9,'El petroleo y sus beneficios',5),(10,'Química IV',5),(11,'La energía y sus materiales',6),(12,'Ingeniería de las cosas',7);
/*!40000 ALTER TABLE `clases` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `grupos`
--

DROP TABLE IF EXISTS `grupos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `grupos` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Clave` varchar(45) NOT NULL,
  `IdCarrera` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_grupo_carrera_idx` (`IdCarrera`),
  CONSTRAINT `fk_grupo_carrera` FOREIGN KEY (`IdCarrera`) REFERENCES `carreras` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `grupos`
--

LOCK TABLES `grupos` WRITE;
/*!40000 ALTER TABLE `grupos` DISABLE KEYS */;
INSERT INTO `grupos` VALUES (1,'91G',1),(2,'92G',1),(3,'72G',1),(4,'22D',3),(5,'72P',4),(6,'11T',5),(7,'52M',2);
/*!40000 ALTER TABLE `grupos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `mensajes`
--

DROP TABLE IF EXISTS `mensajes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `mensajes` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Mensaje` longtext NOT NULL,
  `IdEmisor` int DEFAULT NULL,
  `IdRemitente` int DEFAULT NULL,
  `Fecha` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `fk_user_emisor_idx` (`IdEmisor`),
  KEY `fk_user_remitente_idx` (`IdRemitente`),
  CONSTRAINT `fk_user_emisor` FOREIGN KEY (`IdEmisor`) REFERENCES `usuarios` (`Id`),
  CONSTRAINT `fk_user_remitente` FOREIGN KEY (`IdRemitente`) REFERENCES `usuarios` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `mensajes`
--

LOCK TABLES `mensajes` WRITE;
/*!40000 ALTER TABLE `mensajes` DISABLE KEYS */;
INSERT INTO `mensajes` VALUES (7,'Haciendo pruebas con alumnos de sistemas',12,1,'2022-11-07 09:53:08'),(8,'Haciendo pruebas con alumnos de sistemas',12,2,'2022-11-07 09:53:12'),(9,'Haciendo pruebas con alumnos de sistemas',12,3,'2022-11-07 09:53:19'),(10,'Haciendo pruebas con alumnos de sistemas',12,1,'2022-11-07 09:54:04');
/*!40000 ALTER TABLE `mensajes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usuarios`
--

DROP TABLE IF EXISTS `usuarios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usuarios` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(200) NOT NULL,
  `Rol` varchar(45) NOT NULL,
  `NoControl` varchar(45) NOT NULL,
  `Correo` varchar(300) NOT NULL,
  `Password` varchar(8) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=20 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuarios`
--

LOCK TABLES `usuarios` WRITE;
/*!40000 ALTER TABLE `usuarios` DISABLE KEYS */;
INSERT INTO `usuarios` VALUES (1,'Karla Verónica Lópeaz Tovar','Estudiante','181G0138','181G0138@rcarbonifera.tecnm.mx','181G0138'),(2,'Luis Enrique Castilleja Tristán','Estudiante','181G0231','181G0231@rcarbonifera.tecnm.mx','181G0231'),(3,'Abraham Antonio Torres Martínez','Estudiante','181G0533','181G0533@rcarbonifera.tecnm.mx','181G0533'),(4,'Jacob Asael Barrera','Estudiante','181G0133','181G0133@rcarbonifera.tecnm.mx','181G0233'),(5,'Julio César Salazar','Estudiante','181G0233','181G0233@rcarbonifera.tecnm.mx',''),(6,'Carlos Cuautle Zacamitzin','Estudiante','191G0138','',''),(7,'Miguel Ángel Villafán','Estudiante','191G0139','',''),(8,'Dariela Judith García Zuñiga ','Estudiante','181G0531','',''),(9,'Humberto Ramos Ríos','Estudiante','181G0432','',''),(10,'Jorge Moreno','Estudiante','181G0431','',''),(11,'Job Armendaríz López','Estudiante','202G0138','',''),(12,'Héctor Javier Padilla Lara','Docente','2906','2906@rcarbonifera.tecnm.mx','2906'),(13,'José Carlos Ordoñez','Docente','2907','',''),(14,'Esperanza Ledezma','Docente','2908','',''),(15,'Adriana Ramírez','Docente','2909','',''),(16,'Juan Carlos Sifuentes','Docente','2910','',''),(17,'Oscar Rául Sánchez','Administrativo','2911','',''),(18,'Adrián Garza','Administrativo','2912','',''),(19,'Claudia Garza','Administrativo','2913','','');
/*!40000 ALTER TABLE `usuarios` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usuarios_clases`
--

DROP TABLE IF EXISTS `usuarios_clases`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usuarios_clases` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `IdEstudiante` int DEFAULT NULL,
  `IdClase` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_clase_datos_idx` (`IdClase`),
  KEY `fk_datos_clase_idx` (`IdEstudiante`),
  CONSTRAINT `fk_clase` FOREIGN KEY (`IdClase`) REFERENCES `clases` (`Id`),
  CONSTRAINT `fk_user` FOREIGN KEY (`IdEstudiante`) REFERENCES `usuarios` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuarios_clases`
--

LOCK TABLES `usuarios_clases` WRITE;
/*!40000 ALTER TABLE `usuarios_clases` DISABLE KEYS */;
INSERT INTO `usuarios_clases` VALUES (1,1,1),(2,1,2),(3,1,3),(4,2,4),(5,2,1),(6,3,4),(7,3,2);
/*!40000 ALTER TABLE `usuarios_clases` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-11-07 11:21:10
