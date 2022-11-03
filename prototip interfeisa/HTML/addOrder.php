<?php
//add file

$target_dir = "uploads/";
$target_file = $target_dir . basename($_FILES["filename"]["name"]);
move_uploaded_file($_FILES["filename"]["tmp_name"], $target_file);


//connect
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
    isset ($_POST["group"]) && isset ($_POST["email"])&& isset($_POST["tag"]))
{
    $tag = $_POST["tag"];
    $name =" ";
    $surname = " ";
    $patronimic = " ";
    $fac = " ";
    $group = " ";
    $email = " ";
    $name =$_POST["name"];
    $surname = $_POST["surname"];
    $patronimic = $_POST["patronimic"];
    $fac = $_POST["fac"];
    $group = $_POST["group"];
    $email = $_POST["email"];
}

echo $target_file;
$query = "insert into dev.req_front values ('$tag','$name','$surname','$patronimic','$email','$fac','$group','$target_file',0);";
$result = pg_query($dbconn,$query );

pg_close($dbconn);

?>
