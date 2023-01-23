function OpenModalPartner(externalCode) {
    fetch(`/Home/OpenModalPartner?ExternalCode=${externalCode}`)
        .then(response => response.text())
        .then(data => {
            $('#partnerModal .modal-body').html(data);
            $('#partnerModal').modal('show');
        })
        .catch(error => console.log(error));
}