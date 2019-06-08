// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(()=>{
    let connection = new signalR.HubConnectionBuilder().withUrl("/signalServer").build()

    connection.start()

    connection.on("refreshEmployees",function(){
        loadData()
    })

    loadData();

    function loadData(){
        var tr = ''

        $.ajax({
            url: '/Home/GetEmployees',
            method: 'GET',
            success: (result)=>{
                $.each(result,(k,v)=>{
                    tr = tr + `<tr>
                        <td>${v.id}</td>
                        <td>${v.name}</td>
                        <td>${v.age}</td>
                    </tr>`
                })

                $("#tableBody").html(tr)
            },
            error: (error)=>{
                console.log(error)
            }
        })
    }
})