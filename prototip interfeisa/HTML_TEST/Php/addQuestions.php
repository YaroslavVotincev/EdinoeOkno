<?php
if (isset ($_POST["name"]) && isset ($_POST["surname"]) &&
    isset ($_POST["patronimic"])  && isset ($_POST["fac"]) &&
    isset ($_POST["question"]) && isset ($_POST["email"])&& isset($_POST["subject"]))
{
    $subject = $_POST["subject"];
    $name =$_POST["name"];
    $surname = $_POST["surname"];
    $patronimic = $_POST["patronimic"];
    $fac = $_POST["fac"];
    $question = $_POST["question"];
    $email = $_POST["email"];
}
$host='26.137.232.44';
$db = 'EdinoeOkno';
$username = 'Stas';
$password = '1';
$dbconn = pg_connect("host=$host port=5432 dbname=$db user=$username password=$password");
$query = " ('$name','$surname','$patronimic','$fac','$email','$subject','$question') ";
$query2 = "insert into dev1.questions (first_name, last_name, patronymic, faculty_code, email, subject, body)
values " .$query;
$result = pg_query($dbconn,$query2);
pg_close($dbconn);
$new_url = '../Main.html';
header('Location: '.$new_url);
?>

