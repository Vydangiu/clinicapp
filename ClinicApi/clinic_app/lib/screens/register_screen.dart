import 'package:flutter/material.dart';
import '../services/auth_service.dart';
import '../widgets/common_widgets.dart';

class RegisterScreen extends StatefulWidget {
  const RegisterScreen({super.key});

  @override
  State<RegisterScreen> createState() => _RegisterScreenState();
}

class _RegisterScreenState extends State<RegisterScreen> {
  final _auth = AuthService();
  final _user = TextEditingController();
  final _pass = TextEditingController(text: 'Pknd123@');
  int? _roleId;
  List<Map<String, dynamic>> _roles = [];
  bool _loading = false;

  @override
  void initState() {
    super.initState();
    _loadRoles();
  }

  Future<void> _loadRoles() async {
    try {
      final roles = await _auth.roleOptions();
      setState(() => _roles = roles);
    } catch (e) {
      if (mounted) showError(context, e);
    }
  }

  Future<void> _submit() async {
    if (_roleId == null) {
      showError(context, 'Chưa chọn vai trò');
      return;
    }
    setState(() => _loading = true);
    try {
      await _auth.register(_user.text.trim(), _pass.text, _roleId!);
      if (!mounted) return;
      showOk(context, 'Tạo tài khoản thành công');
      Navigator.pop(context);
    } catch (e) {
      showError(context, e);
    } finally {
      if (mounted) setState(() => _loading = false);
    }
  }

  @override
  void dispose() {
    _user.dispose();
    _pass.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Tạo tài khoản (Admin)')),
      body: ListView(
        padding: const EdgeInsets.all(16),
        children: [
          TextField(
            controller: _user,
            decoration: const InputDecoration(labelText: 'Username'),
          ),
          const SizedBox(height: 12),
          TextField(
            controller: _pass,
            decoration: const InputDecoration(labelText: 'Password'),
          ),
          const SizedBox(height: 12),
          DropdownButtonFormField<int>(
            initialValue: _roleId,
            items: _roles
                .map(
                  (r) => DropdownMenuItem<int>(
                    value: r['roleId'],
                    child: Text(r['roleName']),
                  ),
                )
                .toList(),
            onChanged: (v) => setState(() => _roleId = v),
            decoration: const InputDecoration(labelText: 'Vai trò'),
          ),
          const SizedBox(height: 16),
          FilledButton(
            onPressed: _loading ? null : _submit,
            child: _loading
                ? const CircularProgressIndicator()
                : const Text('Tạo tài khoản'),
          ),
        ],
      ),
    );
  }
}
