function AddGoal() {
    //GoalName GoalDueBy GoalDescription
    let GoalName = document.getElementById("GoalName").value
    let GoalDueBy = document.getElementById("GoalDueBy").value
    let GoalDescription = document.getElementById("GoalDescription").value
    let GoalNumber = document.getElementById("GoalsTable").childElementCount
    document.getElementById("GoalsTable").innerHTML += "" +
        "<tr id='" + GoalNumber.toString() + "'>" +
        "<td><input id='GoalsList[" + GoalNumber.toString() + "].Name' name='GoalsList[" + GoalNumber.toString() + "].Name' required value='" + GoalName + "' /></td>" +
        "<td><input id='GoalsList[" + GoalNumber.toString() + "].Description' name='GoalsList[" + GoalNumber.toString() + "].Description' required value='" + GoalDescription + "' /></td>" +
        "<td><input id='GoalsList[" + GoalNumber.toString() + "].GoalDueBy'  name='GoalsList[" + GoalNumber.toString() + "].GoalDueBy' required type='date' value='" + GoalDueBy + "'/></td>" +
        "<td><button class='btn-sm btn-danger' type='button' onclick='DeleteGoal()' id='btn-" + GoalNumber.toString() + "'>Delete</button></td>" +
        "<input id='GoalsList[" + GoalNumber.toString() + "].Id' class='d-none' value=''/>"
        "</tr>"
}

function DeleteGoal() { 
    if (document.getElementById("GoalsList[" + this.id.split("btn-")[1] + "].Id").value == "") {
        document.removeChild(document.getElementById(this.id.split("btn-")[1]))
    }
    else {
        document.getElementById("GoalsList[" + this.id.split("btn-")[1] + "].Name").value = "DELETE_ME"
        document.getElementById(this.id.split("btn-")[1]).classList.add("d-none")
    }
}

function DeleteProject(event) {
    if (confirm("Do you want to delete this item?")) {
        window.location = '?DeleteItem=' + event.target.id
    }
}