<?php
$host='26.137.232.44';
$db = 'EdinoeOkno';
$username = 'forms_guy';
$password = '1';
$tableName = "forms.response";
$dbconn = pg_connect("host=$host port=5432 dbname=$db user=$username password=$password");
$uid= uniqid();
$text="";
$req = "";
foreach ($_POST as $key => $value) {
    if(is_array($value))
    {
        if($value[0] == "radio-dop")
        {
            $req = $value[1] . "'" . $value[2]. "'";
            $q = "insert into $tableName (respondent,id_question,id_answer,text_input) values ('$uid',$req);";echo $q;echo '<br>';
            $r = pg_query($dbconn,$q);
            $req = "";
        }
        elseif($value[0]=="check-box-dop")
        {
            for($i= 1;$i<sizeof($value)-1;$i++)
            {
                $req = $req . $value[$i] . " " ."'". $value[sizeof($value)-1]."'";
            $q = "insert into $tableName (respondent,id_question,id_answer,text_input) values ('$uid',$req);";echo $q;echo '<br>';
            $r = pg_query($dbconn,$q);
            $req = "";
            }

        }
        elseif($value[0]=="check-box")
        {
            for($i= 1;$i<sizeof($value);$i++)
            {
                $req = $req . $value[$i];
                $q = "insert into $tableName(respondent,id_question,id_answer) values ('$uid',$req);";echo $q;echo '<br>';
                $r = pg_query($dbconn,$q);
                $req = "";
            }

        }
        else
        {
            $req = $value[0] . "'". $value[1]. "'";
            $q = "insert into $tableName (respondent,id_question,id_answer,text_input) values ('$uid',$req);";echo $q;echo '<br>';
            $r = pg_query($dbconn,$q);
            $req = "";

        }
    }else
    {
        $req = $req . $value . " ";
        $q = "insert into $tableName(respondent,id_question,id_answer) values ('$uid',$req);";echo $q;echo '<br>';
        $r = pg_query($dbconn,$q);
        $req = "";
    }



}
pg_close($dbconn);

$new_url = '../../HTML_TEST/FormsLists.php';
header('Location: '.$new_url);
?>