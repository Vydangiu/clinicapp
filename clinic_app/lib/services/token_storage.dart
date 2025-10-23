import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class TokenStorage {
  static const _key = 'access_token';
  final _s = const FlutterSecureStorage();

  Future<void> save(String token) => _s.write(key: _key, value: token);
  Future<String?> read() => _s.read(key: _key);
  Future<void> clear() => _s.delete(key: _key);
}
