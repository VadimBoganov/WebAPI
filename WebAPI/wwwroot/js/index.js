const uri = 'api/employees';
let currData = [];
let currPageNo = 1;
const currPageSize = 5;

let getEmployees = (pageNo = currPageNo, pageSize = currPageSize) => fetch(uri + `/${pageNo}/${pageSize}`)
    .then(response => response.json())
    .then(data => display(data))
    .catch(error => console.log('Unable to get employees...', error))

let addEmployee = () => {
    const firstNameInput = document.querySelector('#first-name');
    const lastNameInput = document.querySelector('#last-name');
    const rateInput = document.querySelector('#rate');
    const typeEmployeeInput = document.querySelector('#descriminator');

    const employee = {
        firstName: firstNameInput.value,
        lastName: lastNameInput.value,
        rate: rateInput.value,
        discriminator: typeEmployeeInput.value
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(employee)
    })
        .then(response => response.json())
        .then(() => getEmployees())
        .catch(error => console.error('Unable to add employee.', error))
}

let display = (data) => {
    const tbody = document.querySelector('#employees');
    tbody.innerHTML = '';

    data.forEach(item => {
        let tr = tbody.insertRow();

        const firstAndLastNames = item.fullName.split(' ');

        let td1 = tr.insertCell(0);
        let firstNameTextNode = document.createTextNode(firstAndLastNames[0]);
        td1.appendChild(firstNameTextNode);

        let td2 = tr.insertCell(1);
        let lastNameTextNode = document.createTextNode(firstAndLastNames[1]);
        td2.appendChild(lastNameTextNode);

        let td3 = tr.insertCell(2);
        let rateNameTextNode = document.createTextNode(item.salary);
        td3.appendChild(rateNameTextNode);
       
    })

    currData = data;
}

let getEmployeeByName = (name) => {
    fetch(uri + '/' + name)
        .then(response => response.json())
        .then((data) => {
            document.querySelector("#employeeByName").innerHTML = JSON.stringify(data);
        })
        .catch(error => console.error('Can not find employee by this ' + name, error))
}

let getMaxSalary = () => {
    fetch(uri + '/maxsalary')
        .then(response => response.json())
        .then((data) => {
            document.querySelector('#maxSalary').innerHTML = JSON.stringify(data);
        })
        .catch(error => console.error('Unable to get max salary', error))
}

let getSumSalaries = () => {
    fetch(uri + '/sumsalary')
        .then(response => response.json())
        .then((data) => {
            document.querySelector('#sumSalaies').innerHTML = JSON.stringify(data);
        })
        .catch(error => console.error('Unable to get sum salaries', error))
}

prev.onclick = () => {
    if (currPageNo == 1) 
        return;

    currPageNo--;
    getEmployees(currPageNo, currPageSize);
}


next.onclick = () => {
    if (Math.ceil(currData.length / currPageSize) < currPageNo) 
        return;

    currPageNo++;
    getEmployees(currPageNo, currPageSize);
}

getEmployees();

