document.addEventListener('DOMContentLoaded', () => {
    
    const boutonPanier = document.querySelectorAll('.ajouter-panier').forEach(btn => {
        btn.addEventListener('click', async function (e) {
            e.preventDefault();
            const id = this.dataset.productId;
            
            if (!id) return null;
            
            this.disabled = true; // désactiver pour éviter double clic
            addProductToCart(id, 1, this);
            
        });
    });
});

async function addProductToCart(ProductId, Quantity, obj) {
    const token = document.querySelector('meta[name="csrf-token"]')?.getAttribute('content');
    try {
        const res = await fetch('/api/Cart/add', {
            method: "POST",
            credentials: "same-origin",
            headers: {
                'Content-Type': 'application/json',
                'X-CSRF-TOKEN': token
            },
            body: JSON.stringify({ ProductId: Number(ProductId), Quantity: Quantity })
        });

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
            obj.disabled = false;
            return;
        }

        if (res.ok && data.success) {
            obj.classList.remove('btn-primary');
            obj.classList.add('btn-success');
            obj.innerHTML = '<i class="bi bi-cart-check"></i> Dans le Panier'
        }
        else {
            alert(data.message || 'Erreur lors de l\'ajout');
            obj.disabled = false;
        }
    }
    catch (err) {
        console.error(err);
        alert("Erreur réseau");
        obj.disabled = false;
    }
}