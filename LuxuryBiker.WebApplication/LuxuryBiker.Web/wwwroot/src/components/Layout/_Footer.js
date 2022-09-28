export default function Footer() {
    const date = new Date().getFullYear();
    return (
        /* Footer  */
        <footer className="sticky-footer bg-white">
            <div className="container my-auto">
                <div className="copyright text-center my-auto">
                    <span>Copyright © LuxuryBiker {date}</span>
                </div>
            </div>
        </footer>
        /* End of Footer  */
    );
}