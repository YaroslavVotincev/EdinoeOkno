<?php
$host='26.137.232.44';
$db = 'EdinoeOkno';
$username = 'Artem';
$password = '1';

$dbconn = pg_connect("host=$host port=5432 dbname=$db user=$username password=$password");

$name =" ";
$surname = " ";
$patronimic = " ";
$fac = " ";
$group = " ";
$email = " ";
if (isset ($_POST["name"]) && isset ($_POST["surname"]) &&
    isset ($_POST["patronimic"])  && isset ($_POST["fac"]) &&
    isset ($_POST["group"]) && isset ($_POST["email"]))
{
    $name =$_POST["name"];
    $surname = $_POST["surname"];
    $patronimic = $_POST["patronimic"];
    $fac = $_POST["fac"];
    $group = $_POST["group"];
    $email = $_POST["email"];
}


$query = "INSERT INTO test(name,surname) VALUES ('$name','$surname')";
$result = pg_query($dbconn,$query );

pg_close($dbconn);

?>