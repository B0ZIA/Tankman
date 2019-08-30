<?php

	$con = mysqli_connect('localhost', 'root', '', 'unityaccess');
	
	//check that connection happend
	if (mysqli_connect_errno())
	{
		echo "1: Connection failed"; //error code #1 - connection failed
		exit();
	}
	
	$username = $_POST["name"];
	$password = $_POST["password"];
	
	//check if name exists
	$namecheckquery = "SELECT username, salt, hash, score FROM players WHERE username='" . $username . "';";
	
	$namecheck = mysqli_query($con, $namecheckquery) or die("2: Name check query failed"); //error code #2 - name check query failed
	if (mysqli_num_rows($namecheck) != 1)
	{
		echo "5: Either no user witch name, or more that one"; //error clde #5 - numbers of names matching != 1
		exit();
	}
	
	//get login form query
	$existinginfo = mysqli_fetch_assoc($namecheck);
	$salt = $existinginfo["salt"];
	$hash = $existinginfo["hash"];
	
	$loginhash = crypt($password, $salt);
	if ($hash != $loginhash)
	{
		echo "6: Incorrect password"; //error code #6 - password does not hash to match table
		exit();
	}
	
	echo "0\t" . $existinginfo["score"];
	
	

?>