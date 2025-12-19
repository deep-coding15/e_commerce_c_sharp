document.addEventListener('DOMContentLoaded', () => {
    // Interactivite de quantité (plus ou moins)
    document.querySelectorAll('.quantities_actions').forEach(group => {
        const minusBtn = group.querySelector('button:first-child');
        const plusBtn = group.querySelector('button:last-child');
        const input = group.querySelector('input[type="number"]');
        const item = group.closest('.cart-item');
        console.log('item: ', item);
        var id = item.dataset.productid;
        console.log('id: ', id);
        if(!item)
            return;

        minusBtn.addEventListener('click', () => {
            let val = parseInt(input.value) || 1;
            const min = parseInt(input.min) || 1;
            if (val > min) input.value = val - 1;
            handleProductToCart(id, 1, -1);
        });

        plusBtn.addEventListener('click', () => {
            let val = parseInt(input.value) || 1;
            input.value = val + 1;
            handleProductToCart(id, 1, 1);
        });

        //se déclenche uniquement quand le champ perd le focus après une modification 
        input.addEventListener('change', () => { 
            
        });
    });

    // Interactivité de suppression
    const trash_action = document.querySelectorAll('.trash-action');
    trash_action.forEach(btnTrash => {
        btnTrash.addEventListener('click', () => {
            const item = btnTrash.closest('.cart-item');
            if(!item)
                return;
            item.remove();            
        });
    });
});

async function handleProductToCart(ProductId, Quantity, delta) {
    const token = document.querySelector('meta[name="csrf-token"]')?.getAttribute('content');
    try {
        var res;
        if(delta > 0){
            res = await fetch('/api/Cart/add', {
                method: "POST",
                credentials: "same-origin",
                headers: {
                    'Content-Type': 'application/json',
                    'X-CSRF-TOKEN': token
                },
                body: JSON.stringify({ ProductId: Number(ProductId), Quantity: Quantity })
            });
        }
        else if (delta < 0) {
            res = await fetch('/api/Cart/suppr', {
                method: "POST",
                credentials: "same-origin",
                headers: {
                    'Content-Type': 'application/json',
                    'X-CSRF-TOKEN': token
                },
                body: JSON.stringify({ ProductId: Number(ProductId), Quantity: Quantity })
            });
        }

        const contentType = res.headers.get("content-type") || "";

        const raw = await res.text();

        let data = null;

        if (contentType.includes("application/json")) {
            data = JSON.parse(raw);
        } else {
            console.error("Réponse non JSON: ", res.status, raw);
            throw new Error("Réponse serveur non-JSON");
        }

        //const data =  await res.json();
        if (!res.ok) {
            alert(data?.message ?? `Erreur HTTP ${res.status}`);
            return;
        }
        
        if (res.ok && data.success) {
            alert(data?.message ?? `Code HTTP ${res.status}`);            
        }
        else {
            alert(data.message || 'Erreur lors de l\'ajout');
        }
    }
    catch (err) {
        console.error(err);
        alert("Erreur réseau");
    }
}