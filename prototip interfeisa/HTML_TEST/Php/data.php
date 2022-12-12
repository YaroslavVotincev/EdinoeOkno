<?php
$host='26.137.232.44';
$db = 'EdinoeOkno';
$username = 'Stas';
$password = '1';
$dbconn = pg_connect("host=$host port=5432 dbname=$db user=$username password=$password");
$query = "select html_code from forms.forms;";
$result = pg_query($dbconn,$query );


//$data = $result;
//echo json_encode($data);

//pg_close($dbconn);
?>
