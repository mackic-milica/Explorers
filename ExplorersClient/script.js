// podaci od interesa
var host = "https://localhost:";
var port = "44346/";
var projekatEndpoint = "api/projects/"
var istrazivacEndpoint = "api/explorers/";
var loginEndpoint = "api/authentication/login";
var registerEndpoint = "api/authentication/register";
var formAction = "Create";
var editingId;
var jwt_token;

function validateRegisterForm(username, email, password, confirmPassword) {
	if (username.length === 0) {
		alert("Username field can not be empty.");
		return false;
	} else if (email.length === 0) {
		alert("Email field can not be empty.");
		return false;
	} else if (password.length === 0) {
		alert("Password field can not be empty.");
		return false;
	} else if (confirmPassword.length === 0) {
		alert("Confirm password field can not be empty.");
		return false;
	} else if (password !== confirmPassword) {
		alert("Password value and confirm password value should match.");
		return false;
	}
	return true;
}

function registerUser() {
	var username = document.getElementById("usernameRegister").value;
	var email = document.getElementById("emailRegister").value;
	var password = document.getElementById("passwordRegister").value;
	var confirmPassword = document.getElementById("confirmPasswordRegister").value;

	if (validateRegisterForm(username, email, password, confirmPassword)) {
		var url = host + port + registerEndpoint;
		var sendData = { "Username": username, "Email": email, "Password": password };
		fetch(url, { method: "POST", headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(sendData) })
			.then((response) => {
				if (response.status === 200) {
					document.getElementById("registerForm").reset();
					console.log("Successful registration");
					alert("Successful registration");
					showLogin();
				} else {
					console.log("Error occured with code " + response.status);
					console.log(response);
					alert("Error occured!");
					response.text().then(text => { console.log(text); })
				}
			})
			.catch(error => console.log(error));
	}
	return false;
}

function showLogin() {
	document.getElementById("data").style.display = "block";
	document.getElementById("formDiv").style.display = "none";
	document.getElementById("loginFormDiv").style.display = "block";
	document.getElementById("registerFormDiv").style.display = "none";
	document.getElementById("loggedIn").style.display = "none";
	document.getElementById("notLoggedIn").style.display = "none";
}

// prikaz forme za registraciju
function showRegistration() {
	document.getElementById("formDiv").style.display = "none";
	document.getElementById("loginFormDiv").style.display = "none";
	document.getElementById("registerFormDiv").style.display = "block";
    document.getElementById("notLoggedIn").style.display = "none";
	//document.getElementById("logout").style.display = "none";
}

function validateLoginForm(username, password) {
	if (username.length === 0) {
		alert("Username field can not be empty.");
		return false;
	} else if (password.length === 0) {
		alert("Password field can not be empty.");
		return false;
	}
	return true;
}

function loginUser() {
	var username = document.getElementById("usernameLogin").value;
	var password = document.getElementById("passwordLogin").value;

	if (validateLoginForm(username, password)) {
		var url = host + port + loginEndpoint;
		var sendData = { "Username": username, "Password": password };
		fetch(url, { method: "POST", headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(sendData) })
			.then((response) => {
				if (response.status === 200) {
					console.log("Successful login");
					alert("Successful login");
					response.json().then(function (data) {
						console.log(data);
						document.getElementById("info").innerHTML = "Currently logged in user: <i>" + data.username + "<i/>.";
						document.getElementById("loggedIn").style.display = "block";
						document.getElementById("data").style.display = "block";
						document.getElementById("notLoggedIn").style.display = "none";
						document.getElementById("loginFormDiv").style.display = "none";
						document.getElementById("formDiv").style.display = "none";
					

						// koristimo Window sessionStorage Property za cuvanje key/value parova u browser-u
						// sessionStorage cuva podatke za samo jednu sesiju
						// podaci će se obrisati kad se tab browser-a zatvori
						// (postoji i localStorage koji čuva podatke bez datuma njihovog "isteka")
						// dobavljanje tokena: token = sessionStorage.getItem(data.token);
						//sessionStorage.setItem("token", data.token);
						jwt_token = data.token;
						loadExplorers();
					});
				} else {
					console.log("Error occured with code " + response.status);
					console.log(response);
					alert("Desila se greska!");
				}
			})
			.catch(error => console.log(error));
	}
	return false;
}

function loadExplorers(){
document.getElementById("data").style.display = "block";
document.getElementById("formDiv").style.display = "none";

var requestUrl = host + port + istrazivacEndpoint;
	console.log("URL zahteva: " + requestUrl);
	var headers = {};
	if (jwt_token) {
		headers.Authorization = 'Bearer ' + jwt_token;			// headers.Authorization = 'Bearer ' + sessionStorage.getItem(data.token);
	}
	console.log(headers);
	fetch(requestUrl, { headers: headers })
		.then((response) => {
			if (response.status === 200) {
				response.json().then(setExplorers);
			} else {
				console.log("Error occured with code " + response.status);
				showError();
			}
		})
		.catch(error => console.log(error));
};

function setExplorers(data){
    var container = document.getElementById("data");
	container.innerHTML = "";

	console.log("Data received from search:");


	console.log(data);

	// ispis naslova
	var div = document.createElement("div");
	var h1 = document.createElement("h1");
	var headingText = document.createTextNode("Istrazivaci");
	h1.appendChild(headingText);
	div.appendChild(h1);

	// ispis tabele
	var table = document.createElement("table");
	//table.className = "table table-hover";

	var header = createHeader();
	table.append(header);

	var tableBody = document.createElement("tbody");

	for (var i = 0; i < data.length; i++) {
		// prikazujemo novi red u tabeli
		var row = document.createElement("tr");
		// prikaz podataka
		
		row.appendChild(createTableCell(data[i].name));
        row.appendChild(createTableCell(data[i].lastName));
        row.appendChild(createTableCell(data[i].birthYear));
        row.appendChild(createTableCell(data[i].projectName));
		if (jwt_token) {
			row.appendChild(createTableCell(data[i].salary));

			// prikaz dugmadi za izmenu i brisanje
			var stringId = data[i].id.toString();

			var buttonEdit = document.createElement("button");
			buttonEdit.name = stringId;
			buttonEdit.addEventListener("click", editExplorer);
			buttonEdit.className = "btn btn-warning";
			var buttonEditText = document.createTextNode("Edit");
			buttonEdit.appendChild(buttonEditText);
			var buttonEditCell = document.createElement("td");
			buttonEditCell.appendChild(buttonEdit);
			row.appendChild(buttonEditCell);

			var buttonDelete = document.createElement("button");
			buttonDelete.name = stringId;
			buttonDelete.addEventListener("click", deleteExplorer);
			buttonDelete.className = "btn btn-danger";
			var buttonDeleteText = document.createTextNode("Delete");
			buttonDelete.appendChild(buttonDeleteText);
			var buttonDeleteCell = document.createElement("td");
			buttonDeleteCell.appendChild(buttonDelete);
			row.appendChild(buttonDeleteCell);
		}
		tableBody.appendChild(row);
	}

	table.appendChild(tableBody);
	div.appendChild(table);

	// prikaz forme
	//if (jwt_token) {
	//	document.getElementById("
	//}// oov sam zakomentarisala da mi se ne bi pojavljivalo odma na strani kada se ulogujem nego tek kada pritisnem na edit
	// ispis novog sadrzaja
	container.appendChild(div);

}

function showError() {
	var container = document.getElementById("data");
	container.innerHTML = "";

	var div = document.createElement("div");
	var h1 = document.createElement("h1");
	var errorText = document.createTextNode("Greska prilikom preuzimanja podataka!");

	h1.appendChild(errorText);
	div.appendChild(h1);
	container.append(div);
}

function createHeader() {
	var thead = document.createElement("thead");
	var row = document.createElement("tr");
	
	row.appendChild(createTableHeaderCell("Ime"));
	row.appendChild(createTableHeaderCell("Prezime"));
    row.appendChild(createTableHeaderCell("Godina rodjenja"));
	row.appendChild(createTableHeaderCell("Projekat"));

	if (jwt_token) {
		row.appendChild(createTableHeaderCell("Zarada (din.)"));
		row.appendChild(createTableHeaderCell("Izmena"));
		row.appendChild(createTableHeaderCell("Brisanje"));
	
	}

	thead.appendChild(row);
	return thead;
}

function createTableHeaderCell(text) {
	var cell = document.createElement("th");
	var cellText = document.createTextNode(text);
	cell.appendChild(cellText);
	return cell;
}

function createTableCell(text) {
	var cell = document.createElement("td");
	var cellText = document.createTextNode(text);
	cell.appendChild(cellText);
	return cell;
}

function deleteExplorer() {
	// izvlacimo {id}
	var deleteID = this.name;
	// saljemo zahtev 
	var url = host + port + istrazivacEndpoint + deleteID.toString();
	var headers = { 'Content-Type': 'application/json' };
	if (jwt_token) {
		headers.Authorization = 'Bearer ' + jwt_token;
	}

	fetch(url, { method: "DELETE", headers: headers})
		.then((response) => {
			if (response.status === 204) {
				console.log("Successful action");
				refreshTable();
			} else {
				console.log("Error occured with code " + response.status);
				alert("Error occured!");
			}
		})
		.catch(error => console.log(error));
};



// izmena istrazivaca
function editExplorer() {
	// izvlacimo id
    document.getElementById("formDiv").style.display = "block";
    document.getElementById("registerFormDiv").style.display = "none";
    document.getElementById("loginFormDiv").style.display = "none";
    document.getElementById("loggedIn").style.display = "none";
    document.getElementById("notLoggedIn").style.display = "none";
    document.getElementById("data").style.display = "none";
	var editId = this.name;
	// saljemo zahtev da dobavimo tog istrazivaca

	var url = host + port + istrazivacEndpoint + editId.toString();
	var headers = { };
	if (jwt_token) {
		headers.Authorization = 'Bearer ' + jwt_token;
	}

	fetch(url, { headers: headers})
		.then((response) => {
			if (response.status === 200) {
				console.log("Successful action");
				response.json().then(data => {
					var name = document.getElementById("explorerName").value = data.name;
					var lastName =  document.getElementById("explorerLastName").value = data.lastName;
					var birthYear = document.getElementById("explorerBirthYear").value = data.birthYear;
					var salary = document.getElementById("explorerSalary").value = data.salary;

					if (!validateInputForm(name, lastName, birthYear, salary)) {
                        return false;
                    }
					editingId = data.id;
					formAction = "Update";
					
				});
			} else {
				formAction = "Create";
				console.log("Error occured with code " + response.status);
				alert("Error occured!");
			}
		})
		.catch(error => console.log(error));
        loadProjectsForDropDown();
};

function validateInputForm(name, lastName, birthYear, salary){
	if (name.length === 0) {
		alert("Ime mora biti uneto.");
		return false;
	} else if(name.length < 2 || name.length > 50) {
		alert("Ime mora imati izmedju 2 i 50 karaktera.");
		return false;
	} else if (lastName.length === 0) {
		alert("Prezime mora biti uneto.");
		return false;
	} else if(lastName.length < 2 || lastName.length > 80) {
		alert("Prezime mora imati izmedju 2 i 80 karaktera.");
		return false;
	} else if (birthYear.length === 0) {
		alert("Godina rodjenja mora biti uneta.");
		return false;
	} else if (birthYear < 1900 || birthYear > 2024) {
		alert("Godina rodjenja mora biti u intervalu od 1900 i 2024.");
		return false;
	} else if (salary.length === 0) {
		alert("Zarada mora biti uneta.");
		return false;
	} else if (salary < 10000 || salary > 500000 ) {
		alert("Zarada mora biti izmedju 10.000 i 500.000.");
		return false;
	}
	return true;

}

function loadProjectsForDropDown() {
	// ucitavanje projekata
	var requestUrl = host + port + projekatEndpoint;
    

	console.log("URL zahteva: " + requestUrl);

	var headers = {};
	if (jwt_token) {
		headers.Authorization = 'Bearer ' + jwt_token;
	}

	fetch(requestUrl, {headers: headers})
		.then((response) => {
			if (response.status === 200) {
				response.json().then(setProjectsInDropdown);
			} else {
				console.log("Error occured with code " + response.status);
			}
		})
		.catch(error => console.log(error));
};

// metoda za postavljanje projekata u padajuci meni
function setProjectsInDropdown(data) {
	var dropdown = document.getElementById("explorerProject");
	dropdown.innerHTML = "";
    console.log("Podaci:" + data);
	for (var i = 0; i < data.length; i++) {
		var option = document.createElement("option");
		option.value = data[i].id;
		var text = document.createTextNode(data[i].name);
		option.appendChild(text);
		dropdown.appendChild(option);
	}
}

// osvezi prikaz tabele
function refreshTable() {
	// cistim formu
	document.getElementById("explorerName").value = "";
	document.getElementById("explorerLastName").value = "";
	document.getElementById("explorerBirthYear").value = "";
	document.getElementById("explorerSalary").value = "";
	document.getElementById("explorerProject").value = "";
	// osvezavam
	loadExplorers();
};

function showPasswordRegister(){
    var x = document.getElementById("passwordRegister");
    var y = document.getElementById("confirmPasswordRegister");
    if (x.type === "password" && y.type === "password") {
        x.type = "text";
        y.type = "text";
    } else {
        x.type = "password";
        y.type = "password";
    }
}

function showingPasswordLogin(){ 
    var x = document.getElementById("passwordLogin");
    if (x.type === "password") {
      x.type = "text";
    } else {
      x.type = "password";
    }
  }

  function homePage(){
	jwt_token = undefined;
    document.getElementById("notLoggedIn").style.display = "block";
    document.getElementById("data").style.display = "block";
    document.getElementById("loggedIn").style.display = "none";
    document.getElementById("loginFormDiv").style.display = "none";
    document.getElementById("registerFormDiv").style.display = "none";
    document.getElementById("formDiv").style.display = "none";
	loadExplorers();
    }


    function submitExplorerForm(){
        var name = document.getElementById("explorerName").value;
        var lastName = document.getElementById("explorerLastName").value;
        var birthYear = document.getElementById("explorerBirthYear").value;
        var salary = document.getElementById("explorerSalary").value;
        var explorerProject = document.getElementById("explorerProject").value;
       
	   
		
        //u zavisnosti od akcije pripremam objekat
		var httpAction;
        var sendData;
        var url;
        
        if (formAction === "Create") {
            httpAction = "POST";
            url = host + port + istrazivacEndpoint;
            sendData = {
                "name": name,
                "lastName": lastName,
                "birthYear": birthYear,
                "salary": salary,
                "projectId": explorerProject
            };
        }
        else { 
            httpAction = "PUT";
            url = host + port + istrazivacEndpoint + editingId.toString();
            sendData = {
                "id": editingId,
                "name": name,
                "lastName": lastName,
                "birthYear": birthYear,
                "salary": salary,
                "projectId": explorerProject
            };
        }
    
        console.log("Objekat za slanje");
        console.log(sendData);
        var headers = { 'Content-Type': 'application/json' };
        if (jwt_token) {
            headers.Authorization = 'Bearer ' + jwt_token;
        }

		if(!validateInputForm(name, lastName, birthYear, salary)){
			return false;
		}
        fetch(url, { method: httpAction, headers: headers, body: JSON.stringify(sendData) })
            .then((response) => {
                if (response.status === 200 || response.status === 201) {
                    console.log("Successful action");
                    formAction = "Create";
                    refreshTable();
                } else {
                    console.log("Error occured with code " + response.status);
                    alert("Desila se greska!");
                }
            })
            .catch(error => console.log(error));
            document.getElementById("loggedIn").style.display = "block";
		
        return false;
    }

	function back(){
		refreshTable();
		document.getElementById("formDiv").style.display = "none";
		document.getElementById("loggedIn").style.display = "block";
	}

	function validateSearch(min, max){
		if (min.length === 0) {
			alert("Minimalna zarada mora biti uneta.");
			return false;
		} else if (max.length === 0) {
			alert("Maksimalna zarada mora biti uneta.");
			return false;
		} else if (min < 0) {
			alert("Minimalna zarada mora biti veca od 0.");
			return false;
		} else if (max < 0) {
			alert("Maksimalna zarada mora biti veca od 0.");
			return false;
		} else if( min > max) {
			alert("Maksimalna zarada mora biti veca od minimalne zarade.");
			return false;
		}
		return true;

	}

	function search(){

		var httpAction;
		var sendData;
		var url;
	
		var najmanjeDin = document.getElementById("najmanjeDin").value;
		var najviseDin = document.getElementById("najviseDin").value;

		najmanjeDin = normalizeDecimal(najmanjeDin);
		najviseDin = normalizeDecimal(najviseDin);

		if (!validateSearch(najmanjeDin, najviseDin)) {
			return false;
		}
	
		url = host + port + "api/pretraga?min=" + najmanjeDin + "&max="+ najviseDin;
	
		console.log("URL zahteva: " + url);
	
		var headers = {};
		if (jwt_token) {
			headers.Authorization = 'Bearer ' + jwt_token;
		}

		var headers = { 'Content-Type': 'application/json', 'charset': 'utf-8' };
		if (jwt_token) {
			headers.Authorization = 'Bearer ' + jwt_token;
		}
	
		var body = JSON.stringify({ min: najmanjeDin, max: najviseDin });
	
	
		fetch(url, { method: "POST", headers: headers, body: body })
			.then((response) => {
				if (response.status === 200) {
					console.log(response);
					response.json().then(setExplorers);
				} else {
					console.log("Error occured with code " + response.status);
					showError();
				}
			})
			.catch(error => console.log(error));
		return false;
	};
	
	function normalizeDecimal(value) {
		let number = parseFloat(value);
		if (!isNaN(number)) {
			return number.toFixed(2);
		}
		return value;
	}
	
	