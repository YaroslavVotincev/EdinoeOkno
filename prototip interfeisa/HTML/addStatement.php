<?php
if (isset ($_POST["name"]) && isset ($_POST["surname"]) &&
    isset ($_POST["patronimic"])  && isset ($_POST["fac"]) &&
    isset ($_POST["group"]) && isset ($_POST["email"])&& isset($_POST["tag"]))
{
    $tag = $_POST["tag"];
    $name =$_POST["name"];
    $surname = $_POST["surname"];
    $patronimic = $_POST["patronimic"];
    $fac = $_POST["fac"];
    $group = $_POST["group"];
    $email = $_POST["email"];

}
$host='26.137.232.44';
$db = 'EdinoeOkno';
$username = 'Artem';
$password = '1';
$tableName = "dev1.req_front";
$dbconn = pg_connect("host=$host port=5432 dbname=$db user=$username password=$password");
//(request_code, first_name, last_name, patronymic, email, faculty_code, student_group, doc_amount, dir_name, public_url)
$query = "insert into $tableName values ('$tag','$name','$surname','$patronimic','$email','$fac','$group','0','0','0');";
$result = pg_query($dbconn,$query );
pg_close($dbconn);
$new_url = '../HTML/Main.html';
header('Location: '.$new_url);
?>

